using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

public class LibraryInteractables : MonoBehaviour
{
    private UIManager m_uiManager;
    private GameManager m_gameManager;
    [TextArea]
    [SerializeField]
    private string m_noItemDialogueText; //diálogo que vai ser exibido quando o player já pegou o item
    private bool m_isIn; //pega se o player está dentro do collider
    [SerializeField]
    private GameObject m_pressEObj;
    private PlayerMovement m_player;
    private SpriteRenderer m_sr;
    [SerializeField]
    private List<Sprite> m_openSprites = new List<Sprite>();

    private bool m_gotItem = false;
    [SerializeField]
    private bool m_isBookshelf;

    [SerializeField]
    private Item m_itemToGive,
                 m_secondItemToGive;

    private UnityAction m_gotNewItem;

    private bool m_gotFirstItem = false;

    private void Awake()
    {
        m_sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        m_gameManager = GameManager.instance;
        m_uiManager = UIManager.instance;
    }
    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            if (m_gotItem)
            {
                Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_noItemDialogueText, UIManager.instance.GetColor(0), m_uiManager.GetVoice(0)[0], m_uiManager.GetVoice(0)[1], m_uiManager.GetVoice(0)[2]).CancelWith(gameObject));
            }
            else
            {
                if (!m_isBookshelf)
                {
                    m_gotNewItem += GetSecondItem;
                    m_gameManager.gotJigsaw.AddListener(m_gotNewItem);
                    UIManager.instance.NewItem(m_itemToGive);
                    m_sr.sprite = m_openSprites[0];
                    m_gotFirstItem = true;
                }
                else
                {
                    UIManager.instance.NewItem(m_itemToGive);                   
                    m_gotItem = true;
                }
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isIn = true;
            m_pressEObj.SetActive(true);
            m_player = other.GetComponent<PlayerMovement>();
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

    public void GetSecondItem()
    {
        if(m_gotFirstItem && !m_gotItem)
        {
            m_gameManager.gotJigsaw.RemoveListener(m_gotNewItem);
            UIManager.instance.NewItem(m_itemToGive);
            m_sr.sprite = m_openSprites[1];
            m_gotItem = true;
        }
      
    }
}

