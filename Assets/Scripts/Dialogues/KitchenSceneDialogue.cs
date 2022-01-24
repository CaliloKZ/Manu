using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using UnityEngine.Playables;

public class KitchenSceneDialogue : MonoBehaviour
{
    [TextArea][SerializeField]
    private List<string> m_dialogueLines = new List<string>();

    private DialogueManager m_dialogueManager;

    private PlayableDirector m_timeline;

    private void Awake()
    {
        m_timeline = GetComponent<PlayableDirector>();
        m_timeline.played += TimelinePlayed;
        m_timeline.stopped += TimelineStopped;
    }
    private void Start()
    {
        m_dialogueManager = DialogueManager.instance;
    }

    public void StartDialogue()
    {
        Timing.RunCoroutine(Dialogue().CancelWith(gameObject));
    }

    IEnumerator<float> Dialogue()
    {
        for(int i = 0; i < m_dialogueLines.Count; i++)
        {
            yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(m_dialogueLines[i]).CancelWith(gameObject)));
            yield return Timing.WaitForSeconds(2f);
        }
        m_dialogueManager.StopDialogue();
        yield return Timing.WaitForOneFrame;
    }

    void TimelinePlayed(PlayableDirector obj)
    {
        GameManager.instance.ChangeCanMove(false);
    }

    void TimelineStopped(PlayableDirector obj)
    {
        GameManager.instance.ChangeCanMove(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            m_timeline.Play();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
