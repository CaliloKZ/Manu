using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField]
    private GameObject m_roomToLoad,
                       m_currentRoom;
    [SerializeField]
    private RoomManager m_roomManager;

    [SerializeField]
    private int m_startPosIndex;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            m_roomManager.LoadRoom(m_roomToLoad, m_currentRoom, m_startPosIndex, false);
        }
    }
}
