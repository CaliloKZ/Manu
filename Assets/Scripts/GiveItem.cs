using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

public class GiveItem : MonoBehaviour
{
    private UIManager m_uiManager;
    private SpriteRenderer m_sr;
    private DialogueManager m_dialogueManager;
    [TextArea][SerializeField]
    private string m_findItemDialogueText; //diálogo que vai ser exibido quando o player interagir
    [TextArea][SerializeField]
    private string m_gotItemDialogueText; //diálogo se o player já tiver pegado o item
    private bool m_isIn; //pega se o player está dentro do collider
    [SerializeField]
    private GameObject m_pressEObj;

    [SerializeField]
    private Item m_itemToGive;

    private bool m_gotItem;

    private UnityAction m_endDialogueAction;

    [SerializeField]
    private bool m_isBookshelf;
    [SerializeField]
    private Sprite m_emptyBookshelf;

    private void Awake()
    {
        m_sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        m_dialogueManager = DialogueManager.instance;
        m_uiManager = UIManager.instance;
    }
    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            if (m_gotItem)
                Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_gotItemDialogueText, m_uiManager.GetColor(1), m_uiManager.GetVoice(1)[0], m_uiManager.GetVoice(1)[1], m_uiManager.GetVoice(1)[2]).CancelWith(gameObject));
            else
               StartDialogue();
        }
    }

    void StartDialogue()
    {
        if (m_isBookshelf)
            m_sr.sprite = m_emptyBookshelf;

        m_endDialogueAction += GetItem;
        m_dialogueManager.dialogueEnded.AddListener(m_endDialogueAction);
        Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_findItemDialogueText, m_uiManager.GetColor(1), m_uiManager.GetVoice(1)[0], m_uiManager.GetVoice(1)[1], m_uiManager.GetVoice(1)[2]).CancelWith(gameObject));
    }
    void GetItem()
    {
        m_gotItem = true;
        m_dialogueManager.dialogueEnded.RemoveListener(m_endDialogueAction);
        UIManager.instance.NewItem(m_itemToGive);
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
}
