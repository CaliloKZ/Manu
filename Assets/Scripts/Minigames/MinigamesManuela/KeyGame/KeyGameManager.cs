using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyGameManager : MonoBehaviour
{
    private GameManager m_gameManager;
    private bool m_leftSideFinished = false,
                 m_rightSideFinished = false;

    [SerializeField]
    private GameObject m_player,
                       m_playerCam,
                       m_keyGamePlayer;

    private void Start()
    {
        m_gameManager = GameManager.instance;
    }

    public void StartMinigame()
    {
        //UIManager.instance.ChangeRoom(1f);
        m_player.SetActive(false);
        m_playerCam.SetActive(false);
    }

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
        //UIManager.instance.ChangeRoom(2f);
        m_gameManager.BookshelfMinigameEnded();
        Destroy(gameObject);

    }
}
