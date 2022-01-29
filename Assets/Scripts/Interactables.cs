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

    private void Start()
    {
        if (GameManager.instance.GetIsManuel())
            m_fontColor = UIManager.instance.GetManuelColor();
        else
            m_fontColor = UIManager.instance.GetManuelaColor();
    }
    private void Update()
    {
        if(m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_itemDialogueText, m_fontColor));        
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

