using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

public class KitchenMomDialogue : MonoBehaviour
{
    private DialogueManager m_dialogueManager;
    private GameManager m_gameManager;
    private BoxCollider2D m_collider;
    [SerializeField]
    private List<DialogueText> m_dialoguePartOne = new List<DialogueText>();
    [SerializeField]
    private List<DialogueText> m_dialoguePartTwo = new List<DialogueText>();
    [SerializeField]
    private GameObject m_dialogueCam;
    [SerializeField]
    private Animator m_momAnim;

    private UnityAction m_endDialogue;

    private void Awake()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        m_dialogueManager = DialogueManager.instance;
        m_gameManager = GameManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_gameManager.ChangeCanMove(false);
            m_collider.enabled = false;
            Timing.RunCoroutine(StartScene().CancelWith(gameObject));
        }
    }

    IEnumerator<float> StartScene()
    {
        m_endDialogue += EndPartOne;
        m_dialogueCam.SetActive(true);
        m_gameManager.GetPlayerCam().SetActive(false);
        yield return Timing.WaitForSeconds(0.5f);
        m_dialogueManager.dialogueEnded.AddListener(m_endDialogue);
        Timing.RunCoroutine(Dialogue(m_dialoguePartOne).CancelWith(gameObject));
    }

    IEnumerator<float> Dialogue(List<DialogueText> dialogues)
    {
        m_dialogueManager.DialogueState(true);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(dialogues).CancelWith(gameObject)));
    }

    void EndPartOne()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_endDialogue);
        m_gameManager.ChangeCanMove(false);
        m_gameManager.GetPlayerCam().SetActive(true);
        m_dialogueCam.SetActive(false);   
        Timing.RunCoroutine(EndPartOneRoutine().CancelWith(gameObject));
    }

    IEnumerator<float> EndPartOneRoutine()
    {
        m_momAnim.SetTrigger("Walk");
        yield return Timing.WaitForSeconds(3.5f);
        Timing.RunCoroutine(Dialogue(m_dialoguePartTwo).CancelWith(gameObject));
    }

    
}
