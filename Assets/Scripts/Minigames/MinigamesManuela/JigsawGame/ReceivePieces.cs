using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReceivePieces : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private JigsawGameManager m_jigManager;
    [SerializeField]
    private int m_idPiece;

    private PiecesController m_droppedPiece;

    public void OnDrop(PointerEventData eventData)
    {
        m_droppedPiece = m_jigManager.pieceSelected;
        Debug.Log("onDrop");
        if (m_droppedPiece.GetID() == m_idPiece)
        {
            m_droppedPiece.SetPosition(transform);
            gameObject.SetActive(false);
        }
    }
}
