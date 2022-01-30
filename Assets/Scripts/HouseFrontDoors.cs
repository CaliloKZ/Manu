using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using MEC;

public class HouseFrontDoors : MonoBehaviour
{
    private DialogueManager m_dialogueManager;
    private bool m_isIn;
    [SerializeField]
    private GameObject m_pressEObj;

    [TextArea][SerializeField]
    private string m_doorDialogueText;

    private Color m_fontColor;
    private List<string> m_voices = new List<string>();

    private UnityAction m_endDialogue;

    private void Start()
    {
        m_dialogueManager = DialogueManager.instance;
        m_fontColor = UIManager.instance.GetColor(1);
        m_voices = UIManager.instance.GetVoice(1);
    }

    private void Update()
    {
        if (m_isIn && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        m_endDialogue += EnterHouse;
        m_dialogueManager.dialogueEnded.AddListener(m_endDialogue);
        Timing.RunCoroutine(DialogueManager.instance.Dialogue(m_doorDialogueText, m_fontColor, m_voices[0], m_voices[1], m_voices[2]));
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

    public void EnterHouse()
    {
        SoundManager.instance.StopMusic();
        m_dialogueManager.dialogueEnded.RemoveListener(m_endDialogue);
        SceneManager.LoadScene("ElHouseScene");
    }
}
