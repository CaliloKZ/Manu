using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private Animator m_anim;
    [SerializeField]
    private GameObject m_body;
    [SerializeField]
    private float m_moveSpeed;
    private float m_horizontalMove;
    private bool m_FacingRight;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField]
    private float m_MovementSmoothing = .05f;	// How much to smooth out the movement

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        m_FacingRight = m_body.transform.localScale.x > 0 ? false : true;
    }

    private void Update()
    {
        if (GameManager.instance.canMove)
        {
            m_horizontalMove = Input.GetAxisRaw("Horizontal") * m_moveSpeed;
            m_anim.SetFloat("Speed", Mathf.Abs(m_horizontalMove));
        } 
    }

    private void FixedUpdate()
    {
        Move(m_horizontalMove * Time.fixedDeltaTime);
    }

    void Move(float move)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, m_rb.velocity.y);
        // And then smoothing it out and applying it to the character
        m_rb.velocity = Vector3.SmoothDamp(m_rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }

       
    }

    public void StopMove()
    {
        m_rb.velocity = Vector2.zero;
        m_anim.SetFloat("Speed", 0);
        m_horizontalMove = 0;
    }
    void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void MovePlayerToPos()
    {
        GameManager.instance.ChangeCanMove(false);
        StopMove();
        m_rb.position = GameManager.instance.GetFinalManuelaPos().position;
    }
}

