using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyGameManager : MonoBehaviour
{
    private bool m_leftSideFinished = false,
                 m_rightSideFinished = false;

    [SerializeField]
    private GameObject m_firstFloor,
                       m_player,
                       m_playerCam;

    public void FinishedLeftSide()
    {
        if (m_rightSideFinished)
        {
            EndGame();
        }
        else
        {
            m_leftSideFinished = true;
        }
    }

    public void FinishedRightSide()
    {
        if (m_leftSideFinished)
        {
            EndGame();
        }
        else
        {
            m_rightSideFinished = true;
        }
    }

    public void EndGame()
    {
        UIManager.instance.ChangeRoom(2f);
        m_playerCam.SetActive(true);
        m_player.SetActive(true);
        m_firstFloor.SetActive(true);
        Destroy(gameObject);

    }
}
