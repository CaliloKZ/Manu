using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MEC;

public class StealMinigameController : MonoBehaviour
{
    private UIManager m_uiManager;
    private bool m_isIn; //pega se o player está dentro do collider
    [TextArea][SerializeField]
    private string m_stealMinigameFailDialogue;
    [SerializeField]
    private GameObject m_pressEObj;
    [SerializeField]
    private Color m_fontColor;

    [SerializeField]
    private Item m_money;

    [SerializeField]
    private GameObject m_stealMinigame;

    private UnityAction m_endDialogue;

    private void Start()
    {
        m_uiManager = UIManager.instance;
    }
    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            m_stealMinigame.SetActive(true);
            GameManager.instance.ChangeCanMove(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isIn = true;
            m_pressEObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_isIn = false;
            m_pressEObj.SetActive(false);
        }
    }

    public void Sucess()
    {
        UIManager.instance.NewItem(m_money);
        GetComponentInParent<NPCInteraction>().Stolen();
    }

    public void Failed()
    {
        m_endDialogue += DialogueEnded;
        DialogueManager.instance.dialogueEnded.AddListener(m_endDialogue);
        Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_stealMinigameFailDialogue, m_fontColor, m_uiManager.GetVoice(3)[0], m_uiManager.GetVoice(3)[1], m_uiManager.GetVoice(3)[2]).CancelWith(gameObject));
    }

    void DialogueEnded()
    {
        DialogueManager.instance.dialogueEnded.RemoveListener(m_endDialogue);
        GameManager.instance.GameOver();
    }
}

