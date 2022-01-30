using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

public class BreadNPC : MonoBehaviour
{

    private DialogueManager m_dialogueManager;
    private UIManager m_uiManager;

    [SerializeField]
    private List<DialogueText> m_firstEncounterDialogue = new List<DialogueText>();
    [TextArea]
    [SerializeField]
    private string m_afterFirstEncounterDialogue;
    [SerializeField]
    private List<DialogueText> m_secondEncounterDialogue = new List<DialogueText>();
    [TextArea]
    [SerializeField]
    private string m_afterSecondEncounterDialogue;

    private bool m_isIn; //pega se o player está dentro do collider
    [SerializeField]
    private GameObject m_pressEObj;
    [SerializeField]
    private Color m_fontColor;

    private bool m_firstDialogueFinished,
                 m_stealMinigameFinished,
                 m_secondDialogueFinished;

    private UnityAction m_endDialogue;

    private void Start()
    {
        m_dialogueManager = DialogueManager.instance;
        m_uiManager = UIManager.instance;
    }
    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        if (m_secondDialogueFinished)
        {
            Timing.RunCoroutine(m_dialogueManager.Dialogue(m_afterSecondEncounterDialogue, m_fontColor, m_uiManager.GetVoice(3)[0], m_uiManager.GetVoice(3)[1], m_uiManager.GetVoice(3)[2]).CancelWith(gameObject));
        }
        else if (GameManager.instance.finishedStealMinigame)
        {
            m_endDialogue -= StartStealMinigame;
            m_endDialogue += ActivateAlleyGuy;
            m_dialogueManager.dialogueEnded.AddListener(m_endDialogue);
            m_dialogueManager.DialogueState(true);
            Timing.RunCoroutine(m_dialogueManager.Dialogue(m_secondEncounterDialogue).CancelWith(gameObject));
            m_secondDialogueFinished = true;
            return;
        }
        else if (m_firstDialogueFinished)
        {
            Timing.RunCoroutine(m_dialogueManager.Dialogue(m_afterFirstEncounterDialogue, m_fontColor, m_uiManager.GetVoice(3)[0], m_uiManager.GetVoice(3)[1], m_uiManager.GetVoice(3)[2]).CancelWith(gameObject));
            return;
        }
        else
        {
            m_endDialogue += StartStealMinigame;
            m_dialogueManager.dialogueEnded.AddListener(m_endDialogue);
            m_dialogueManager.DialogueState(true);
            Timing.RunCoroutine(m_dialogueManager.Dialogue(m_firstEncounterDialogue).CancelWith(gameObject));
            m_firstDialogueFinished = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isIn = true;
            m_pressEObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isIn = false;
            m_pressEObj.SetActive(false);
        }
    }

    void StartStealMinigame()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_endDialogue);
        GameManager.instance.StartStealMinigame();
    }

    void ActivateAlleyGuy()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_endDialogue);
        GameManager.instance.ActivateAlleyGuy();
    }
}

