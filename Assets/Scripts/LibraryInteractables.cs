using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class LibraryInteractables : MonoBehaviour
{
    [TextArea]
    [SerializeField]
    private string m_findItemDialogueText, //diálogo que vai ser exibido quando o player achar o item
                   m_noItemDialogueText; //diálogo que vai ser exibido quando o player já pegou o item
    private bool m_isIn; //pega se o player está dentro do collider
    [SerializeField]
    private GameObject m_pressEObj;
    private PlayerMovement m_player;

    private List<GameObject> m_objsToGive = new List<GameObject>();

    private bool m_gotItem = false;


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
                Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_findItemDialogueText, UIManager.instance.GetManuelaColor()));
                //show new item
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

