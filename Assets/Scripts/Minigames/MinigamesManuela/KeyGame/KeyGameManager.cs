using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;


public class KeyGameManager : MonoBehaviour
{
    private GameManager m_gameManager;
    private DialogueManager m_dialogueManager;
    private UIManager m_uiManager;
    private bool m_leftSideFinished = false,
                 m_rightSideFinished = false;

    [SerializeField]
    private List<DialogueText> m_dialogueBeforeStart = new List<DialogueText>();
    [TextArea][SerializeField]
    private List<string> m_secondPartDialogue = new List<string>();
    [TextArea][SerializeField]
    private List<string> m_endGameDialogue = new List<string>();

    private UnityAction m_endFirstDialogue,
                        m_endSecondDialogue,
                        m_endFinalDialogue;

    [SerializeField]
    private GameObject m_player,
                       m_playerCam,
                       m_keyGamePlayer,
                       m_keyGameCam,
                       m_gamePanel;

    [SerializeField]
    private MomController m_momController;

    [SerializeField]
    private Item m_key;

    private void Start()
    {
        m_endFirstDialogue += StartMinigame;
        m_endSecondDialogue += SecondPart;
        m_endFinalDialogue += End;
        m_gameManager = GameManager.instance;
        m_dialogueManager = DialogueManager.instance;
        m_uiManager = UIManager.instance;
    }
    public void StartDialogue()
    {
        m_keyGameCam.SetActive(true);
        m_playerCam.SetActive(false);
        Timing.RunCoroutine(BeforeGameDialogue().CancelWith(gameObject));
    }

    IEnumerator<float> BeforeGameDialogue()
    {
        m_dialogueManager.dialogueEnded.AddListener(m_endFirstDialogue);
        m_dialogueManager.DialogueState(true);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(m_dialogueBeforeStart).CancelWith(gameObject)));
    }

    void StartMinigame()
    {
        UIManager.instance.ActivateBookshelfTexts(true);
        m_player.SetActive(false);
        m_gamePanel.SetActive(true);
        m_momController.PauseResumeChange(false);      
    }

    void StartSecondDialogue()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_endFirstDialogue);
        Timing.RunCoroutine(SecondPartDialogue().CancelWith(gameObject));

    }

    IEnumerator<float> SecondPartDialogue()
    {
        m_dialogueManager.dialogueEnded.AddListener(m_endSecondDialogue);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(m_secondPartDialogue[0], m_uiManager.GetColor(0), m_uiManager.GetVoice(0)[0], m_uiManager.GetVoice(0)[1], m_uiManager.GetVoice(0)[2]).CancelWith(gameObject)));
    }

    void SecondPart()
    {
        m_keyGamePlayer.GetComponent<KeyGamePlayerController>().SecondPart();
        m_momController.PauseResumeChange(false);
    }

    public void FinishedLeftSide()
    {
        m_momController.PauseResumeChange(true);
        m_gameManager.ChangeCanMove(false);
        m_leftSideFinished = true;
        StartSecondDialogue();
    }

    public void FinishedRightSide()
    {
        EndGame();
    }

    void EndGame()
    {
        m_dialogueManager.dialogueEnded.AddListener(m_endSecondDialogue);
        m_momController.PauseResumeChange(true);
        Timing.RunCoroutine(EndGameDialogue().CancelWith(gameObject));
    }
    void End()
    {
        UIManager.instance.NewItem(m_key);
        m_gameManager.BookshelfMinigameEnded();
    }

    IEnumerator<float> EndGameDialogue()
    {
        m_dialogueManager.dialogueEnded.AddListener(m_endFinalDialogue);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(m_endGameDialogue[0], m_uiManager.GetColor(0), m_uiManager.GetVoice(0)[0], m_uiManager.GetVoice(0)[1], m_uiManager.GetVoice(0)[2]).CancelWith(gameObject)));      
    }
}
