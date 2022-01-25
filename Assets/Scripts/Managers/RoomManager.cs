using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private GameManager m_gameManager;
    private void Start()
    {
        m_gameManager = GameManager.instance;
    }
    public void LoadRoom(GameObject roomToLoad, GameObject roomToUnload, int startPos)
    {
        Room _newRoom = roomToLoad.GetComponent<Room>();
        roomToLoad.SetActive(true);
        UIManager.instance.ChangeRoom(1f);
        m_gameManager.SetPlayerPos(_newRoom.GetStartPos(startPos));
        m_gameManager.SetCameraConfiner(_newRoom.GetConfiner());
        roomToUnload.SetActive(false);
    }
}
