using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class LibraryInteractables : MonoBehaviour
{
    [TextArea]
    [SerializeField]
    private string m_noItemDialogueText; //diálogo que vai ser exibido quando o player já pegou o item
    private bool m_isIn; //pega se o player está dentro do collider
    [SerializeField]
    private GameObject m_pressEObj;
    private PlayerMovement m_player;

    private List<GameObject> m_objsToGive = new List<GameObject>();

    private bool m_gotItem = false;
    [SerializeField]
    private bool m_isBookshelf;

    [SerializeField]
    private Item m_itemToGive,
                 m_secondItemToGive;


    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            if (m_gotItem)
            {
                Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_noItemDialogueText, UIManager.instance.GetManuelaColor()));
            }
            else
            {
                if (!m_isBookshelf)
                {
                    UIManager.instance.NewItem(m_itemToGive);
                    GameManager.instance.FoundJigsawItems();
                    UIManager.instance.NewItem(m_secondItemToGive);
                    GameManager.instance.FoundJigsawItems();
                    m_gotItem = true;
                }
                else
                {
                    UIManager.instance.NewItem(m_itemToGive);
                    GameManager.instance.FoundJigsawItems();
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
            DialogueManager.instance.StopDialogue();
        }
    }
}

