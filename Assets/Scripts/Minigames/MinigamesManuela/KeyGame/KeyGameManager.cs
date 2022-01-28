using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;


public class KeyGameManager : MonoBehaviour
{
    private GameManager m_gameManager;
    private DialogueManager m_dialogueManager;
    private bool m_leftSideFinished = false,
                 m_rightSideFinished = false;

    [TextArea][SerializeField]
    private List<string> m_secondPartDialogue = new List<string>();
    [TextArea][SerializeField]
    private List<string> m_endGameDialogue = new List<string>();

    [SerializeField]
    private GameObject m_player,
                       m_playerCam,
                       m_keyGamePlayer;

    [SerializeField]
    private MomController m_momController;

    private void Start()
    {
        m_gameManager = GameManager.instance;
        m_dialogueManager = DialogueManager.instance;
    }

    public void StartMinigame()
    {
        UIManager.instance.ActivateBookshelfTexts(true);
        m_momController.PauseResumeChange(false);
        m_player.SetActive(false);
        m_playerCam.SetActive(false);
    }

    IEnumerator<float> StartSecondPart()
    {
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(SecondPartDialogue().CancelWith(gameObject)));
        m_keyGamePlayer.GetComponent<KeyGamePlayerController>().SecondPart();
        m_momController.PauseResumeChange(false);
    }

    IEnumerator<float> SecondPartDialogue()
    {
        for(int i = 0; i < m_secondPartDialogue.Count; i++)
        {
            yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(m_secondPartDialogue[i], UIManager.instance.GetManuelaColor()).CancelWith(gameObject)));
            yield return Timing.WaitForSeconds(2f);
        }
        m_dialogueManager.StopDialogue();
    }

    public void FinishedLeftSide()
    {
        m_momController.PauseResumeChange(true);
        m_gameManager.ChangeCanMove(false);
        m_leftSideFinished = true;
        Timing.RunCoroutine(StartSecondPart().CancelWith(gameObject));
    }

    public void FinishedRightSide()
    {
        EndGame();
    }

    public void EndGame()
    {
        m_momController.PauseResumeChange(true);
        Timing.RunCoroutine(EndGameDialogue().CancelWith(gameObject));
    }

    IEnumerator<float> EndGameDialogue()
    {
        for (int i = 0; i < m_endGameDialogue.Count; i++)
        {
            yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(m_endGameDialogue[i], UIManager.instance.GetManuelaColor()).CancelWith(gameObject)));
            yield return Timing.WaitForSeconds(2f);
        }
        m_dialogueManager.StopDialogue();
        m_gameManager.BookshelfMinigameEnded();
    }
}
