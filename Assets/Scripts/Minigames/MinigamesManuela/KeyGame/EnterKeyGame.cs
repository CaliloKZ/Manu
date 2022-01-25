using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterKeyGame : MonoBehaviour
{
    [SerializeField]
    private GameObject m_playerCam,
                       m_player,
                       m_minigamePanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartMinigame();
            Destroy(gameObject);
        }
    }

    void StartMinigame()
    {
        UIManager.instance.ChangeRoom(3f);
        m_playerCam.SetActive(false);
        m_player.SetActive(false);
        m_minigamePanel.SetActive(true);
    }
}
