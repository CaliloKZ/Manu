using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using MEC;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool minigameManuelaOneDone { get; private set; }
    public bool minigameManuelaTwoDone{ get; private set; }

    public bool foundFirstPuzzlePieces { get; private set; } = false;

    [SerializeField]
    private GameObject m_bedroom;
    [SerializeField]
    private GameObject m_player,
                       m_playerCam,
                       m_momCouch;

    [SerializeField]
    private CinemachineConfiner m_mainCamConfiner;

    [SerializeField]
    private GameObject m_bookshelfMinigame,
                       m_bookshelfNormal;
    [SerializeField]
    private List<DialogueText> m_firstPuzzlePieceDialogue = new List<DialogueText>();

    [SerializeField]
    private int m_jigsawItems;

    [SerializeField]
    private GameObject m_jigsawGameCanvas;

    public bool canMove { get; private set; }
    public bool canPause { get; private set; }

    public GameObject GetPlayer()
    {
        return m_player;
    }

    public GameObject GetPlayerCam()
    {
        return m_playerCam;
    }

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

    private void Start()
    {
        m_bedroom.SetActive(true);
        canMove = true;
    }

    private void Update()
    {
        if(canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            //pausar
        }
    }

    public void ChangeCanMove(bool newCanMove)
    {
        canMove = newCanMove;
        m_player.GetComponent<BoxCollider2D>().enabled = newCanMove;
        m_player.GetComponent<PlayerMovement>().StopMove();
    }

    public void BackFromKitchen()
    {
        m_momCouch.SetActive(true);
        m_bookshelfMinigame.SetActive(true);
        m_bookshelfNormal.SetActive(false);
    }

    public void KitchenSpySceneEnded()
    {
        ChangeCanMove(true);
    }

    public void BookshelfMinigameEnded()
    {
        minigameManuelaOneDone = true;
        m_bookshelfNormal.SetActive(true);
        Destroy(m_bookshelfMinigame);
        m_player.SetActive(true);
        m_playerCam.SetActive(true);
        m_momCouch.GetComponent<MomController>().enabled = false;
        var _momAnim = m_momCouch.GetComponent<Animator>();
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

    public void FirstPuzzlePiecesFound()
    {
        foundFirstPuzzlePieces = true;
        DialogueManager.instance.DialogueState(true);
        Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_firstPuzzlePieceDialogue));
    }

    public void FoundJigsawItems()
    {
        m_jigsawItems++;
        if (m_jigsawItems >= 7)
        {
            m_jigsawGameCanvas.SetActive(true);
        }
    }
}
