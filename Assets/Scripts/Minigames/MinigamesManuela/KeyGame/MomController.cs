using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MomController : MonoBehaviour
{
    private RandomNumberGenerator m_RNG;
    private byte[] m_bytes = { 0, 0, 0, 100 };
    private Animator m_anim;
    [SerializeField]
    private KeyGamePlayerController m_keyPlayer;

    [SerializeField]
    private string m_readTrigger,
                   m_susLeftTrigger,
                   m_susRightTrigger,
                   m_lookLeftTrigger,
                   m_lookRightTrigger;

    [Header("Odds to change State")] [Tooltip("")] [Range(0, 100)] [SerializeField]
    private int m_rToSLOdds;
    [Range(0, 100)][SerializeField]
    private int m_rToSROdds,
                m_SLToROdds,
                m_SLToSROdds,
                m_SLToLLOdds,
                m_SRToROdds,
                m_SRToSLOdds,
                m_SRToLROdds;

    [SerializeField]
    private float m_changeStateCooldown;
    private float m_changeStateCount;

    private bool m_canChangeState;

    private MomStates m_currentState;
    private void Start()
    {
        m_anim = GetComponent<Animator>();
        m_RNG = RandomNumberGenerator.Create();
        m_currentState = MomStates.Reading;
        m_rToSLOdds = CalculateOdds(m_rToSLOdds);
        m_rToSROdds = CalculateOdds(m_rToSROdds);
        m_SLToROdds = CalculateOdds(m_SLToROdds);
        m_SLToSROdds = CalculateOdds(m_SLToSROdds);
        m_SLToLLOdds = CalculateOdds(m_SLToLLOdds);
        m_SRToROdds = CalculateOdds(m_SRToROdds);
        m_SRToSLOdds = CalculateOdds(m_SRToSLOdds);
        m_SRToLROdds = CalculateOdds(m_SRToLROdds);
    }

    private void FixedUpdate()
    {
        if (m_canChangeState)
        {
            ChooseNewState();
        }
        else
        {
            m_changeStateCount += Time.fixedDeltaTime;
            if(m_changeStateCount >= m_changeStateCooldown)
            {
                m_canChangeState = true;
                m_changeStateCount = 0;
            }
        }
    }

    int RandomNumber()
    {
        m_RNG.GetBytes(m_bytes, 3, 1);
        return m_bytes[3];
    }

    int CalculateOdds(int initialValue)
    {
        int _newOdds = (255 * initialValue) / 100;
        if (_newOdds > 255)
            _newOdds = 255;
        return _newOdds;
    }

    void ChooseNewState()
    {
        m_anim.ResetTrigger(m_readTrigger);
        m_anim.ResetTrigger(m_susLeftTrigger);
        m_anim.ResetTrigger(m_susRightTrigger);
        m_anim.ResetTrigger(m_lookLeftTrigger);
        m_anim.ResetTrigger(m_lookRightTrigger);
        m_canChangeState = false;
        int _random = RandomNumber();
        switch (m_currentState) 
        {
            case MomStates.Reading:             
                if(_random <= m_rToSLOdds)
                {
                    ChangeState(MomStates.SusLeft);
                }
                else
                {
                    ChangeState(MomStates.SusRight);
                }
                break;
            case MomStates.SusLeft:
                if(_random <= m_SLToROdds)
                {
                    ChangeState(MomStates.Reading);
                }
                else if (_random > m_SLToROdds && _random <= (m_SLToROdds + m_SLToSROdds))
                {
                    ChangeState(MomStates.SusRight);
                }
                else
                {
                    ChangeState(MomStates.LookLeft);
                }
                break;
            case MomStates.SusRight:
                if (_random <= m_SRToROdds)
                {
                    ChangeState(MomStates.Reading);
                }
                else if (_random > m_SRToROdds && _random <= (m_SRToROdds + m_SRToSLOdds))
                {
                    ChangeState(MomStates.SusLeft);
                }
                else
                {
                    ChangeState(MomStates.LookRight);
                }
                break;
            case MomStates.LookLeft:
                ChangeState(MomStates.LookRight);
                break;
            case MomStates.LookRight:
                ChangeState(MomStates.Reading);
                break;
        }
    }
    void ChangeState(MomStates newState)
    {
        m_currentState = newState;
        switch (newState)
        {
            case MomStates.Reading:
                m_anim.SetTrigger(m_readTrigger);
                break;
            case MomStates.SusLeft:
                m_anim.SetTrigger(m_susLeftTrigger);
                break;
            case MomStates.SusRight:
                m_anim.SetTrigger(m_susRightTrigger);
                break;
            case MomStates.LookLeft:
                m_anim.SetTrigger(m_lookLeftTrigger);
                break;
            case MomStates.LookRight:
                m_anim.SetTrigger(m_lookRightTrigger);
                if (!m_keyPlayer.isHidden)
                {
                    GameManager.instance.GameOver();
                }
                break;
        }
        
    }
}
