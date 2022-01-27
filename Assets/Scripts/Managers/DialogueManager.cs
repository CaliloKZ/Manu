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

    [SerializeField]
    private Material m_dialogueFontMat;

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

    public IEnumerator<float> Dialogue(string text, Color fontColor)
    {
        Timing.KillCoroutines("textRoutine");
        m_dialogueFontMat.SetColor("_OutlineColor", fontColor);
        //m_dialogueText.outlineWidth = 0.2f;
       // m_dialogueText.outlineColor = fontColor;
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
