using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

public class AlleyNPC : MonoBehaviour
{

    private DialogueManager m_dialogueManager;
    [SerializeField]
    private RoomManager m_roomManager;
    private Animator m_anim;

    [SerializeField]
    private List<DialogueText> m_firstPartDialogue = new List<DialogueText>();

    [SerializeField]
    private GameObject m_roomToLoad,
                       m_currentRoom;

    private bool m_isIn; //pega se o player está dentro do collider
    [SerializeField]
    private GameObject m_pressEObj;
    [SerializeField]
    private Color m_fontColor;

    private UnityAction m_endDialogue;

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
    }
    private void Start()
    {
        m_dialogueManager = DialogueManager.instance;
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
        m_endDialogue += GoToHouse;
        m_dialogueManager.dialogueEnded.AddListener(m_endDialogue);
        m_dialogueManager.DialogueState(true);
        Timing.RunCoroutine(m_dialogueManager.Dialogue(m_firstPartDialogue).CancelWith(gameObject));
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

    void GoToHouse()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_endDialogue);
        m_roomManager.LoadRoom(m_roomToLoad, m_currentRoom, 0, true);
    }
}

