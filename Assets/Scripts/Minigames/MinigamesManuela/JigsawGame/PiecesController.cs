using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private JigsawGameManager m_jigManager;
    public bool isDragging { get; private set; }

    private CanvasGroup m_canvasGroup;

    [SerializeField]
    private int m_idPiece;
    public int GetID()
    {
        return m_idPiece;
    }

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_jigManager = JigsawGameManager.instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        m_canvasGroup.blocksRaycasts = false;
        m_jigManager.SetSelectedPiece(this);
        Debug.Log("OBD");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        m_canvasGroup.blocksRaycasts = true;
        Debug.Log("OEsD");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OPD");
    }

    public void SetPosition(Transform pos)
    {
        transform.position = pos.position;
        m_canvasGroup.interactable = false;
        m_jigManager.PieceSet();
        this.enabled = false;
    }
}
