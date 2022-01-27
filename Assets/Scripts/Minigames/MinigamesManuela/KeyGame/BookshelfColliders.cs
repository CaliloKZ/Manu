using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookshelfColliders : MonoBehaviour
{
    [SerializeField]
    private KeyGameManager m_manager;

    [SerializeField]
    private float m_maximumCharge,
                  m_chargeSpeed;
    private float m_charge;
    private bool m_isIn;

    [SerializeField]
    private bool m_isLeft;
    private bool m_isCharging,
                 m_canCharge = true;

    [SerializeField]
    private GameObject m_pressE,
                       m_bookshelfSlider;

    private UIManager m_uiManager;

    [SerializeField]
    private GameObject m_EText;

    private void Start()
    {
        m_uiManager = UIManager.instance;
        m_uiManager.SetBookshelfSliderMaximumValue(m_maximumCharge);
    }
    private void Update()
    {
        if (!m_canCharge)
        {
            return;
        }

        if(m_isIn && Input.GetKey(KeyCode.E))
        {
            m_bookshelfSlider.SetActive(true);
            m_isCharging = true;
            m_EText.SetActive(false);
        }
        else
        {
            m_bookshelfSlider.SetActive(false);
            m_isCharging = false;
        }
    }

    private void FixedUpdate()
    {
        if (m_isCharging)
        {
            m_charge += m_chargeSpeed * Time.fixedDeltaTime;
            m_uiManager.UpdateBookshelfSlider(m_isLeft, m_charge);
            if (m_charge >= m_maximumCharge)
            {
                FinishSide();
                m_canCharge = false;
                m_isCharging = false;
            }
        }       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KeyGamePlayer"))
        {
            m_isIn = true;
            m_pressE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("KeyGamePlayer"))
        {
            m_isIn = false;
            m_pressE.SetActive(false);
        }
    }

    void FinishSide()
    {
        m_bookshelfSlider.SetActive(false);
        if (m_isLeft)
            m_manager.FinishedLeftSide();
        else
            m_manager.FinishedRightSide();
    }
}
