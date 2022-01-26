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
    private float m_horizontalMove;

    [SerializeField]
    private List<Transform> m_playerFixedPositions = new List<Transform>(); //0 = hide left, 1 = left, 2 = right, 3 = hide right.

    private int m_posIndex = 1;

    [SerializeField]
    private float m_moveCooldown;
    private float m_moveCooldownCount;
    private bool m_canMove = true;

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
                Move(-1f);           
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(1f);
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

    void Move(float move)
    {
        if(move > 0)
        {
            if(m_posIndex < 3)
            {
                m_posIndex++;
                ChangePosition();
            }               
        }
        else if(move < 0)
        {
            if(m_posIndex > 0)
            {
                m_posIndex--;
                ChangePosition();
            }            
        }        
    }

    void ChangePosition()
    {
        m_rb.position = m_playerFixedPositions[m_posIndex].position;
        if(m_posIndex == 0 || m_posIndex == 3)
        {
            m_anim.SetBool("isHiding", true);
            isHidden = true;
        }
        else if (m_posIndex == 1 || m_posIndex == 2)
        {
            m_anim.SetBool("isHiding", false);
            isHidden = false;
        }
        m_canMove = false;
    }
}


