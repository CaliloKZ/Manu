using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    private Rigidbody2D m_rb;

    [SerializeField]
    private float m_speed;

    public bool isInRightArea { get; private set; }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_rb.velocity = new Vector2(m_speed * Time.fixedDeltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StealGameLimit"))
        {
            m_rb.velocity = -m_rb.velocity;
        }
        else if (other.CompareTag("StealGameRightArea"))
        {
            isInRightArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StealGameRightArea"))
        {
            isInRightArea = false;
        }
    }

    public void Stop()
    {
        Debug.Log("BUT IM WORKING");
        m_rb.velocity = Vector2.zero;
    }
}
