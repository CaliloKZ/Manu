using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Interactables : MonoBehaviour
{
    [TextArea][SerializeField]
    private string m_itemDialogueText; //diálogo que vai ser exibido quando o player interagir
    private bool m_isIn; //pega se o player está dentro do collider
    [SerializeField]
    private GameObject m_pressEObj;
    private PlayerMovement m_player;
    

    private void Update()
    {
        if(m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_itemDialogueText));
            
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

