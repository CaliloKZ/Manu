using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGamePlayerController : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private Animator m_anim;
    private SpriteRenderer m_sRenderer;
    [SerializeField]
    private GameObject m_body;

   //[SerializeField]
    //private List<Transform> m_playerFixedPositions = new List<Transform>(); //0 = hide left, 1 = left, 2 = right, 3 = hide right.

    [SerializeField]
    private Transform m_currentPlayerHidePos,
                      m_currentPlayerSearchPos,
                      m_secondPlayerHidePos,
                      m_secondPlayerSearchPos;

    [SerializeField]
    private GameObject m_HideText,
                       m_SearchText,
                       m_PressEText;

    private int m_posIndex = 1;

    [SerializeField]
    private float m_moveCooldown;
    private float m_moveCooldownCount;
    private bool m_canMove = true;

    [SerializeField]
    private GameObject m_rightArrow,
                       m_leftArrow;

    private bool m_isFirstPartDone,
                 m_isFirstHide = true,
                 m_isFirstSearch = true;

    public bool isHidden { get; private set; }

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_sRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (GameManager.instance.canMove && m_canMove)
        {
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //Move(-1f);
                ChangePosition(!m_isFirstPartDone);
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                //Move(1f);
                ChangePosition(m_isFirstPartDone);
            }
        }       
    }

    private void FixedUpdate()
    {
        if (!m_canMove)
        {
            m_moveCooldownCount += Time.fixedDeltaTime;
            if(m_moveCooldownCount >= m_moveCooldown)
            {
                m_canMove = true;
                m_moveCooldownCount = 0;
            }
        }
    }

    //void Move(float move)
    //{
        //if(move > 0)
        //{
        //    if(m_posIndex < 3)
        //    {
        //        m_posIndex++;
        //        ChangePosition();
        //    }               
        //}
        //else if(move < 0)
        //{
        //    if(m_posIndex > 0)
        //    {
        //        m_posIndex--;
        //        ChangePosition();
        //    }            
        //}        
    //}

    public void SecondPart()
    {
        m_currentPlayerHidePos = m_secondPlayerHidePos;
        m_currentPlayerSearchPos = m_secondPlayerSearchPos;
        m_isFirstPartDone = true;
        m_rb.position = m_currentPlayerSearchPos.position;
        m_leftArrow.SetActive(false);
        m_rightArrow.SetActive(true);
        GameManager.instance.ChangeCanMove(true);
    }

    void ChangePosition(bool hide)
    {
        if (hide && !isHidden)
        {
            if (m_isFirstHide)
            {
                m_PressEText.SetActive(false);
                m_HideText.SetActive(false);
                m_SearchText.SetActive(true);
                m_isFirstHide = false;
            }
            m_rb.position = m_currentPlayerHidePos.position;
            m_leftArrow.SetActive(m_isFirstPartDone);
            m_rightArrow.SetActive(!m_isFirstPartDone);
            m_anim.SetBool("isHiding", true);
            isHidden = true;
        }
        else if(!hide && isHidden)
        {
            if (m_isFirstSearch)
            {
                m_SearchText.SetActive(false);
                m_isFirstSearch = false;
            }
            m_rb.position = m_currentPlayerSearchPos.position;
            m_leftArrow.SetActive(!m_isFirstPartDone);
            m_rightArrow.SetActive(m_isFirstPartDone);
            m_anim.SetBool("isHiding", false);
            isHidden = false;
        }
        else
        {
            return;
        }
        m_canMove = false;
    }

    //void ChangePosition()
    //{
    //    UIManager.instance.ActivateBookshelfTexts(false);
    //    m_rb.position = m_playerFixedPositions[m_posIndex].position;
    //    switch (m_posIndex)
    //    {
    //        case 0:
    //            m_leftArrow.SetActive(false);
    //            m_anim.SetBool("isHiding", true);
    //            isHidden = true;
    //            break;
    //        case 1:
    //            m_leftArrow.SetActive(true);
    //            m_anim.SetBool("isHiding", false);
    //            isHidden = false;
    //            break;
    //        case 2:
    //            m_rightArrow.SetActive(true);
    //            m_anim.SetBool("isHiding", false);
    //            isHidden = false;
    //            break; 
    //        case 3:
    //            m_rightArrow.SetActive(false);
    //            m_anim.SetBool("isHiding", true);
    //            isHidden = true;
    //            break;
    //        default:
    //            break;
    //    }
    //    m_canMove = false;
    //}
}


