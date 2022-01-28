using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private JigsawGameManager m_jigManager;
    public bool isDragging { get; private set; }

    private CanvasGroup m_canvasGroup;

    [SerializeField]
    private GameObject m_parent;


    [SerializeField]
    private int m_idPiece;
    public int GetID()
    {
        return m_idPiece;
    }

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        m_canvasGroup.blocksRaycasts = false;
        m_jigManager.SetSelectedPiece(this);
        transform.SetAsLastSibling();
        Debug.Log("OBD");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp(mousePos.x, m_jigManager.GetMaxMousePos0().position.x, m_jigManager.GetMaxMousePos1().position.x);
        mousePos.y = Mathf.Clamp(mousePos.y, m_jigManager.GetMaxMousePos1().position.y, m_jigManager.GetMaxMousePos0().position.y);
        transform.position = mousePos;

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
