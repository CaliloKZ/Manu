using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            m_roomManager.LoadRoom(m_roomToLoad, m_currentRoom, m_startPosIndex);
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
}
