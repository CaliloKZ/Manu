using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool m_minigameManuelaOneDone,
                 m_minigameManuelaTwoDone;

    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    private CinemachineConfiner m_mainCamConfiner;

    [SerializeField]
    private GameObject m_bookshelfMinigameCollider;

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
        Debug.Log("imhere");
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


    public void KitchenSceneEnded()
    {
        canMove = true;
        m_bookshelfMinigameCollider.SetActive(true);
    }

    public void SetPlayerPos(Transform newPos)
    {
        m_player.transform.position = newPos.position;
    }

    public void SetCameraConfiner(PolygonCollider2D confiner)
    {
        m_mainCamConfiner.m_BoundingShape2D = confiner;
    }
}
