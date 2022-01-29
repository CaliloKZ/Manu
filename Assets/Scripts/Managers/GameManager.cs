using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using MEC;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private DialogueManager m_dialogueManager;

    public bool hasLibraryKey { get; private set; }

    public bool foundFirstPuzzlePieces { get; private set; } = false;

    [SerializeField]
    private bool m_isManuel;

    public bool GetIsManuel()
    {
        return m_isManuel;
    }

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
    [TextArea][SerializeField]
    private string m_foundPhotoDialogue;
    [TextArea][SerializeField]
    private string m_finishedPuzzleDialogue;
    [TextArea][SerializeField]
    private string m_gotOldKeyDialogue;
    [TextArea][SerializeField]
    private string m_gotOldKeyDialogueAfterLibrary;

    [SerializeField]
    private List<DialogueText> m_startPuzzleDialogue = new List<DialogueText>();

    private LibraryInteractables m_obj;

    public int jigsawItems { get; private set; }

    [SerializeField]
    private GameObject m_jigsawGameCanvas;

    [SerializeField]
    private InventoryManager m_inventory;

    public bool canMove { get; private set; }
    public bool canPause { get; private set; }
    public UnityEvent gotJigsaw { get; private set; }

    private bool m_foundLibrary = false;

    private UnityAction m_jigsawDialogueEnded;
    private UnityAction m_photoDialogueEnded;
    private UnityAction m_PuzzleDialogueEnded;

    [SerializeField]
    private Transform m_finalManuelaPos;

    [SerializeField]
    private GameObject m_firstRoomToLoad;

    public Transform GetFinalManuelaPos()
    {
        return m_finalManuelaPos;
    }

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
        gotJigsaw = new UnityEvent();
        m_dialogueManager = DialogueManager.instance;
        m_firstRoomToLoad.SetActive(true);
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
        hasLibraryKey = true;
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
        m_jigsawDialogueEnded += SecondJigsaw;
        foundFirstPuzzlePieces = true;
        m_dialogueManager.dialogueEnded.AddListener(m_jigsawDialogueEnded);
        m_dialogueManager.DialogueState(true);
        Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_firstPuzzlePieceDialogue));
    }

    void SecondJigsaw()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_jigsawDialogueEnded);
        FoundJigsawItem();
    }

    public void FoundJigsawItem()
    {

        jigsawItems++;
        if (jigsawItems >= 7)
        {
            StartPuzzleDialogue();
        }
        ChangeCanMove(true);
        Timing.RunCoroutine(JigsawInvokeDelay().CancelWith(gameObject));
    }

    IEnumerator<float> JigsawInvokeDelay()
    {
        yield return Timing.WaitForSeconds(0.2f);
        gotJigsaw.Invoke();
    }

    public void GotPhoto()
    {
        m_photoDialogueEnded += EndPhotoDialogue;
        m_dialogueManager.dialogueEnded.AddListener(m_photoDialogueEnded);
        Timing.RunCoroutine(m_dialogueManager.Dialogue(m_foundPhotoDialogue, UIManager.instance.GetManuelaColor()).CancelWith(gameObject));
    }

    void EndPhotoDialogue()
    {
        FoundJigsawItem();
        m_dialogueManager.dialogueEnded.RemoveListener(m_photoDialogueEnded);
    }

    void StartPuzzleDialogue()
    {
        m_PuzzleDialogueEnded += StartPuzzle;
        m_dialogueManager.dialogueEnded.AddListener(m_PuzzleDialogueEnded);
        m_dialogueManager.DialogueState(true);
        Timing.RunCoroutine(m_dialogueManager.Dialogue(m_startPuzzleDialogue).CancelWith(gameObject));
    }

    void StartPuzzle()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_PuzzleDialogueEnded);
        m_jigsawGameCanvas.SetActive(true);
        m_player.GetComponent<PlayerMovement>().MovePlayerToPos();
    }

    public void EndPuzzle()
    {
        m_PuzzleDialogueEnded -= StartPuzzle;
        m_PuzzleDialogueEnded += EndManuelaScene;
        m_dialogueManager.dialogueEnded.AddListener(m_PuzzleDialogueEnded);
        Timing.RunCoroutine(m_dialogueManager.Dialogue(m_finishedPuzzleDialogue, UIManager.instance.GetManuelaColor()));
    }

    public void EndManuelaScene()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_PuzzleDialogueEnded);
        m_player.GetComponent<Animator>().SetTrigger("Cry");
        Timing.RunCoroutine(UIManager.instance.FadeInWithDelay(3f, 7.5f).CancelWith(gameObject));
    }

    public void FoundLockedLibrary()
    {
        m_foundLibrary = true;
    }

    public void GotOldKey()
    {
        hasLibraryKey = true;
        if (m_foundLibrary)
            Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_gotOldKeyDialogueAfterLibrary, UIManager.instance.GetManuelColor()).CancelWith(gameObject));
        else
            Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_gotOldKeyDialogue, UIManager.instance.GetManuelColor()).CancelWith(gameObject));
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
