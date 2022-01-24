using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MEC;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField]
    private TextMeshProUGUI m_dialogueText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of DialogueManager was detected. Destroying gameObject.");
            Destroy(gameObject);
        }
    }

    public IEnumerator<float> Dialogue(string text)
    {
        Timing.KillCoroutines("textRoutine");
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(TextAnim(text, 0.01f).CancelWith(gameObject), "textRoutine"));
    }

    public void StopDialogue()
    {
        Timing.KillCoroutines("textRoutine");
        m_dialogueText.text = "";
    }

    IEnumerator<float> TextAnim(string text, float textSpeed)
    {
        m_dialogueText.text = "";
        foreach (char c in text)
        {
            m_dialogueText.text += c;
            yield return Timing.WaitForSeconds(textSpeed);

        }
    }
}
