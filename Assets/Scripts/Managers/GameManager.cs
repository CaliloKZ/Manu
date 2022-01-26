using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool m_minigameManuelaOneDone,
                 m_minigameManuelaTwoDone;

    private bool m_kitchenSpyCutsceneDone,
                 m_kitchenDialogueCutsceneDone;

    [SerializeField]
    private GameObject m_player,
                       m_playerCam,
                       m_mom;

    [SerializeField]
    private CinemachineConfiner m_mainCamConfiner;

    [SerializeField]
    private GameObject m_bookshelfMinigame,
                       m_bookshelfNormal;

    public bool canMove { get; private set; }
    public bool canPause { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of GameManager was found. Destroying gameObject");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            //pausar
        }
    }

    private void Start()
    {
        canMove = true;
    }

    public void ChangeCanMove(bool newCanMove)
    {
        canMove = newCanMove;
    }


    public void KitchenSpySceneEnded()
    {
        canMove = true;
        m_kitchenSpyCutsceneDone = true;
        m_bookshelfMinigame.SetActive(true);
        m_bookshelfNormal.SetActive(false);
    }

    public void BookshelfMinigameEnded()
    {
        m_minigameManuelaOneDone = true;
        m_bookshelfNormal.SetActive(true);
        Destroy(m_bookshelfMinigame);
        //m_bookshelfMinigame.SetActive(false);
        m_player.SetActive(true);
        m_playerCam.SetActive(true);
        m_mom.GetComponent<MomController>().enabled = false;
        var _momAnim = m_mom.GetComponent<Animator>();
        _momAnim.SetTrigger("Read");
        _momAnim.enabled = false;

    }

    public void SetPlayerPos(Transform newPos)
    {
        m_player.transform.position = newPos.position;
    }

    public void SetCameraConfiner(PolygonCollider2D confiner)
    {
        m_mainCamConfiner.m_BoundingShape2D = confiner;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("ElaHouseScene");
    }
}
