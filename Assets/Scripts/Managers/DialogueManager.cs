using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using MEC;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private GameManager m_gameManager;
    private SoundManager m_soundManager;

    [SerializeField]
    private TextMeshProUGUI m_dialogueText;

    [SerializeField]
    private Material m_dialogueFontMat;

    private bool m_isInDialogueState = false;

    private int m_dialogueIndex;
    private int m_maxDialogues;
    private List<DialogueText> m_dialogueTexts;

    public UnityEvent dialogueEnded;

    [SerializeField]
    private GameObject m_nextIcon;

    private int m_random;
    private string m_audioToPlay;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of DialogueManager was detected. Destroying gameObject.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {       
        m_gameManager = GameManager.instance;
        m_soundManager = SoundManager.instance;
    }

    private void Update()
    {
        if (m_isInDialogueState)
        {
            m_gameManager.ChangeCanMove(false);
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                NextDialogue();
            }
        }
    }

    public void DialogueState(bool enter)
    {
        m_dialogueIndex = 0;
        m_isInDialogueState = enter;
    }

    void NextDialogue()
    {
        m_dialogueIndex++;
        if(m_dialogueIndex >= m_maxDialogues)
        {
            StopDialogue();
            m_gameManager.ChangeCanMove(true);
            DialogueState(false);
            dialogueEnded.Invoke();
            return;
        }
        Timing.RunCoroutine(Dialogue(m_dialogueTexts));
    }
    public IEnumerator<float> Dialogue(string text, Color fontColor, string audioName, string audioNameTwo, string audioNameThree)
    {
        m_nextIcon.SetActive(false);
        DialogueState(true);
        m_maxDialogues = 1;
        Timing.KillCoroutines("textRoutine");
        m_dialogueFontMat.SetColor("_OutlineColor", fontColor);
        Timing.RunCoroutine(DialogueSoundRoutine(audioName, audioNameTwo, audioNameThree).CancelWith(gameObject), "voice");
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(TextAnim(text, 0.01f).CancelWith(gameObject), "textRoutine"));
        Timing.KillCoroutines("voice");
        m_nextIcon.SetActive(true);
    }

    public IEnumerator<float> Dialogue(List<DialogueText> dialogueTexts)
    {
        m_dialogueTexts = dialogueTexts;
        m_maxDialogues = m_dialogueTexts.Count;
        Timing.KillCoroutines("textRoutine");
        m_dialogueFontMat.SetColor("_OutlineColor", m_dialogueTexts[m_dialogueIndex].fontColor);
        Timing.RunCoroutine(DialogueSoundRoutine(dialogueTexts[m_dialogueIndex].voices[0], dialogueTexts[m_dialogueIndex].voices[1], dialogueTexts[m_dialogueIndex].voices[2]).CancelWith(gameObject), "voice");
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(TextAnim(dialogueTexts[m_dialogueIndex].text, 0.01f).CancelWith(gameObject), "textRoutine"));
        Timing.KillCoroutines("voice");
    }

    IEnumerator<float> DialogueSoundRoutine(string audioName, string audioNameTwo, string audioNameThree)
    {
        if (!string.IsNullOrWhiteSpace(m_audioToPlay))
        {
            m_soundManager.StopSFX(m_audioToPlay);
        }
        m_random = UnityEngine.Random.Range(0, 3);
        if(m_random == 0)
        {
            m_audioToPlay = audioName;
        }
        else if(m_random == 1)
        {
            m_audioToPlay = audioNameTwo;
        }
        else if (m_random == 2)
        {
            m_audioToPlay = audioNameThree;
        }
        m_soundManager.PlaySFX(m_audioToPlay);
         yield return Timing.WaitForSeconds(0.15f);
        Timing.RunCoroutine(DialogueSoundRoutine(audioName, audioNameTwo, audioNameThree).CancelWith(gameObject), "voice");
    }

    public void StopDialogue()
    {
        m_nextIcon.SetActive(false);
        Timing.KillCoroutines("textRoutine");
        m_dialogueText.text = "";
    }

    IEnumerator<float> TextAnim(string text, float textSpeed)
    {
        m_dialogueText.text = "";
        foreach (char c in text)
        {
            m_dialogueText.text += c;
            yield return Timing.WaitForSeconds(textSpeed);

        }
    }
}
