using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EntryDialogue : MonoBehaviour
{
    private DialogueManager m_dialogueManager;
    [SerializeField]
    private List<DialogueText> m_dialogues = new List<DialogueText>();

    private void Start()
    {
        m_dialogueManager = DialogueManager.instance;
        m_dialogueManager.DialogueState(true);
        Timing.RunCoroutine(StartDialogue().CancelWith(gameObject));
    }

    IEnumerator<float> StartDialogue()
    {
        yield return Timing.WaitForSeconds(1.5f);     
        Timing.RunCoroutine(m_dialogueManager.Dialogue(m_dialogues).CancelWith(gameObject));
    }
}
