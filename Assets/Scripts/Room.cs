using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Room : MonoBehaviour
{
    private DialogueManager m_dialogueManager;
    [SerializeField]
    private PolygonCollider2D m_confiner;
    [SerializeField]
    private List<Transform> m_playerStartPos = new List<Transform>();

    [SerializeField]
    private List<DialogueText> m_dialogueTexts = new List<DialogueText>();

    [SerializeField]
    private bool m_isFirstFloor;

    private void Start()
    {
        m_dialogueManager = DialogueManager.instance;
    }
    public PolygonCollider2D GetConfiner()
    {
        return m_confiner;
    }

    public Transform GetStartPos(int index)
    {
        return m_playerStartPos[index];
    }

    public void StartDialogue()
    {
        if (m_isFirstFloor)
        {
            GameManager.instance.BackFromKitchen();
        }
        GameManager.instance.ChangeCanMove(false);
        Timing.RunCoroutine(Dialogue().CancelWith(gameObject));
    }

    IEnumerator<float> Dialogue()    
    {
        yield return Timing.WaitForSeconds(0.5f);
        m_dialogueManager.DialogueState(true);
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(m_dialogueTexts).CancelWith(gameObject)));
    }



}
