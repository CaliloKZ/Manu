using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private GameManager m_gameManager;

    [SerializeField]
    private bool m_isManuel;
    private void Start()
    {
        m_gameManager = GameManager.instance;
    }
    public void LoadRoom(GameObject roomToLoad, GameObject roomToUnload, int startPos, bool hasDialogue)
    {
        Room _newRoom = roomToLoad.GetComponent<Room>();
        roomToLoad.SetActive(true);
        UIManager.instance.ChangeRoom(1f);
        m_gameManager.SetPlayerPos(_newRoom.GetStartPos(startPos));
        if (m_isManuel)
        {
            m_gameManager.SetMelPos(_newRoom.GetStartPos(startPos));
        }
        m_gameManager.SetCameraConfiner(_newRoom.GetConfiner());
        roomToUnload.SetActive(false);
        if (hasDialogue)
        {
            _newRoom.StartDialogue();
        }

        if (_newRoom.GetStreet() && m_gameManager.stealMinigameOn)
        {
            _newRoom.MinigameOn();
        }
    }
}
