using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class JigsawGameManager : MonoBehaviour
{
    public static JigsawGameManager instance;

    public PiecesController pieceSelected { get; private set; }

    public int m_piecesSetted = 0;
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
            Timing.RunCoroutine(EndJigsawGame().CancelWith(gameObject));
            
            //gameObject.SetActive(false);
        }
    }

    IEnumerator<float> EndJigsawGame()
    {
        GetComponent<Canvas>().enabled = false;
        UIManager.instance.ChangeRoom(1f);
        yield return Timing.WaitForSeconds(1f);
        GameManager.instance.EndPuzzle();
        gameObject.SetActive(false);
    }
}
