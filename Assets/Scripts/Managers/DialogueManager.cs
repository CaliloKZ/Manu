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

    [SerializeField]
    private TextMeshProUGUI m_dialogueText;

    [SerializeField]
    private Material m_dialogueFontMat;

    private bool m_isInDialogueState = false;

    private int m_dialogueIndex;
    private int m_maxDialogues;
    private List<DialogueText> m_dialogueTexts;

    public UnityEvent dialogueEnded;

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
    public IEnumerator<float> Dialogue(string text, Color fontColor)
    {
        DialogueState(true);
        m_maxDialogues = 1;
        Timing.KillCoroutines("textRoutine");
        m_dialogueFontMat.SetColor("_OutlineColor", fontColor);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(TextAnim(text, 0.01f).CancelWith(gameObject), "textRoutine"));      
    }

    public IEnumerator<float> Dialogue(List<DialogueText> dialogueTexts)
    {
        m_dialogueTexts = dialogueTexts;
        m_maxDialogues = m_dialogueTexts.Count;
        Timing.KillCoroutines("textRoutine");
        m_dialogueFontMat.SetColor("_OutlineColor", m_dialogueTexts[m_dialogueIndex].fontColor);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(TextAnim(dialogueTexts[m_dialogueIndex].text, 0.01f).CancelWith(gameObject), "textRoutine"));
    }

    public void StopDialogue()
    {
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
