using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawGameManager : MonoBehaviour
{
    public static JigsawGameManager instance;

    public PiecesController pieceSelected { get; private set; }

    private int m_piecesSetted = 0;
    [SerializeField]
    private int m_piecesNumber;

    [SerializeField]
    private Transform m_maxMousePosMinusXPlusY,
                      m_maxMousePosPlusXMinusY;

    public Transform GetMaxMousePos0()
    {
        return m_maxMousePosPlusXMinusY;
    }
    public Transform GetMaxMousePos1()
    {
        return m_maxMousePosMinusXPlusY;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("iminstance");
        }
        else
        {
            Debug.LogWarning("Another instance of JigsawGameManager was found. Destroying gameObject");
            Destroy(gameObject);
        }
    }
    public void SetSelectedPiece(PiecesController piece)
    {
        pieceSelected = piece;
    }

    public void PieceSet()
    {
        m_piecesSetted++;
        if (m_piecesSetted == m_piecesNumber)
        {
            Debug.Log("endgamejigsaw");
            //endgame
        }
    }

    public void Test()
    {
        Debug.Log("test");
    }
}
