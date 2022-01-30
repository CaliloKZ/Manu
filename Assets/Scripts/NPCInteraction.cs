using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class NPCInteraction : MonoBehaviour
{
    private UIManager m_uiManager;
    [TextArea][SerializeField]
    private string m_interactionDialogueText; //diálogo que vai ser exibido quando o player interagir
    private bool m_isIn; //pega se o player está dentro do collider
    [SerializeField]
    private GameObject m_pressEObj;
    [SerializeField]
    private Color m_fontColor;

    [SerializeField]
    private GameObject m_minigameCollider;

    private bool m_npcStolen;

    private void Start()
    {
        m_uiManager = UIManager.instance;
    }
    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_interactionDialogueText, m_fontColor, m_uiManager.GetVoice(3)[0], m_uiManager.GetVoice(3)[1], m_uiManager.GetVoice(3)[2]));       
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

    public void MinigameIsOn()
    {
        if(!m_npcStolen)
            m_minigameCollider.SetActive(true);
    }

    public void Stolen()
    {
        m_npcStolen = true;
        m_minigameCollider.SetActive(false);
    }
}

