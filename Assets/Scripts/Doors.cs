using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Doors : MonoBehaviour
{
    private bool m_isIn;
    [SerializeField]
    private GameObject m_roomToLoad,
                       m_currentRoom;
    [SerializeField]
    private RoomManager m_roomManager;

    [SerializeField]
    private GameObject m_pressEObj;

    [SerializeField]
    private int m_startPosIndex;

    [SerializeField]
    private bool m_isLocked,
                 m_hasDialogue;

    [TextArea][SerializeField]
    private string m_doorDialogueText;

    private Color m_fontColor;

    private void Start()
    {
        if (GameManager.instance.GetIsManuel())
            m_fontColor = UIManager.instance.GetManuelColor();
        else
            m_fontColor = UIManager.instance.GetManuelaColor();
    }

    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            if (!m_isLocked)
            {
                m_roomManager.LoadRoom(m_roomToLoad, m_currentRoom, m_startPosIndex, m_hasDialogue);
                m_hasDialogue = false;
            }
            else
            {
                if (GameManager.instance.hasLibraryKey)
                {
                    m_isLocked = false;
                    //soundManager key sound
                    m_roomManager.LoadRoom(m_roomToLoad, m_currentRoom, m_startPosIndex, m_hasDialogue);
                    m_hasDialogue = false;
                }
                else
                {
                    Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_doorDialogueText, m_fontColor));
                    GameManager.instance.FoundLockedLibrary();
                }           
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            m_isIn = true;
            m_pressEObj.SetActive(true);
        }              
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            m_isIn = false;
            m_pressEObj.SetActive(false);
        }
    }

    public void ActivateKitchenDialogue()
    {
        m_hasDialogue = true;
    }
}
