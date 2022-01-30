using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class MelController : MonoBehaviour
{
    private GameManager m_gameManager;
    [SerializeField]
    private float m_speed;

    private Rigidbody2D m_rb;
    private Animator m_anim;
    private SpriteRenderer m_sr;

    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)]
    [SerializeField]
    private float m_MovementSmoothing = .05f;	// How much to smooth out the movement

    private float m_horizontalMove;

    [SerializeField]
    private float m_maxDistanceToPlayer;
    private float m_distanceToPlayer;

    [SerializeField]
    private GameObject m_player;
    private PlayerMovement m_playerScript;

    private bool m_petting = false;
    private bool m_isIn;
    [SerializeField]
    private GameObject m_pressE;
    [SerializeField]
    private Transform m_playerPettingPosition;
    private Vector3 m_playerLastPos;
    private Vector3 m_playerLastScale;

    private bool m_canMove = true;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        m_sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        m_gameManager = GameManager.instance;
        m_playerScript = m_player.GetComponent<PlayerMovement>();
        Timing.RunCoroutine(WakeUp().CancelWith(gameObject));
    }

    private void Update()
    {
        m_distanceToPlayer = Vector2.Distance(transform.position, m_player.transform.position);
        if(m_distanceToPlayer >= m_maxDistanceToPlayer && m_canMove)
        {
            if(m_player.transform.position.x > transform.position.x)
            {
                Walk(m_speed * Time.fixedDeltaTime);
                m_anim.SetFloat("Speed", 1);
                Flip(true);
            }
            else if (m_player.transform.position.x < transform.position.x)
            {
                Walk(-1 * m_speed * Time.fixedDeltaTime);
                m_anim.SetFloat("Speed", 1);
                Flip(false);
            }          
        }
        else
        {
            Stop();
            m_anim.SetFloat("Speed", 0);
        }

        if (m_isIn && Input.GetKeyDown(KeyCode.F))
        {
            Timing.RunCoroutine(YouCanPetTheDog().CancelWith(gameObject));        
        }

        if (m_petting && Input.GetKeyDown(KeyCode.F))
        {
            StopPetTheDog();
        }
    }
 
    public void Walk(float move)
    {
        // Move the character by finding the target velocity
         Vector3 targetVelocity = new Vector2(move * 10f, m_rb.velocity.y);
        // And then smoothing it out and applying it to the character
         m_rb.velocity = Vector3.SmoothDamp(m_rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    public void Flip(bool right)
    {
        m_sr.flipX = !right;
    }

    public void Stop()
    {
        m_rb.velocity = Vector2.zero;
    }

    IEnumerator<float> WakeUp()
    {
        yield return Timing.WaitForSeconds(3f);
        m_anim.SetTrigger("Wake");
    }

    IEnumerator<float> YouCanPetTheDog()
    {
        m_playerScript.PetTheDog();
        m_anim.SetBool("isPetting", true);     
        m_playerLastPos = m_player.transform.position;
        m_playerLastScale = m_playerScript.GetScale();
        m_player.transform.localScale = Vector3.one;
        m_player.transform.position = m_playerPettingPosition.position;
        yield return Timing.WaitForSeconds(0.5f);
        m_petting = true;
    }

    void StopPetTheDog()
    {
        m_petting = false;
        m_playerScript.StopPetting();
        m_anim.SetBool("isPetting", false);
        m_player.transform.position = m_playerLastPos;
        m_player.transform.localScale = m_playerLastScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isIn = true;
            m_pressE.SetActive(true);
        }   
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isIn = false;
            m_pressE.SetActive(false);
        }
    }

    public void ChangeCanMove(bool move)
    {
        m_canMove = move;
    }
}
