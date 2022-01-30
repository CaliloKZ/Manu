using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Interactables : MonoBehaviour
{
    [TextArea][SerializeField]
    private string m_itemDialogueText; //diálogo que vai ser exibido quando o player interagir
    private bool m_isIn; //pega se o player está dentro do collider
    [SerializeField]
    private GameObject m_pressEObj;
    private Color m_fontColor;
    private List<string> m_voices;

    private void Start()
    {
        if (GameManager.instance.GetIsManuel())
        {
            m_fontColor = UIManager.instance.GetColor(1);
            m_voices = UIManager.instance.GetVoice(1);
        }
        else
        {
            m_fontColor = UIManager.instance.GetColor(0);
            m_voices = UIManager.instance.GetVoice(0);
        }            
    }
    private void Update()
    {
        if(m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_itemDialogueText, m_fontColor, m_voices[0], m_voices[1], m_voices[2]));        
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
}

