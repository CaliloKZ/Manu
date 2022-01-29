using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using UnityEngine.Playables;
using UnityEngine.Events;

public class LibraryManuelScene : MonoBehaviour
{
    private PlayableDirector m_timeline;

    private DialogueManager m_dialogueManager;

    [SerializeField]
    private List<DialogueText> m_dialogueTexts = new List<DialogueText>();

    private UnityAction m_dialogueEnded;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            m_timeline.Play();
        }
    }
    void Dialogue()
    {
        PauseTimeline();
        m_dialogueEnded += ResumeTimeline;
        m_dialogueManager.dialogueEnded.AddListener(m_dialogueEnded);
        m_dialogueManager.DialogueState(true);
        Timing.RunCoroutine(m_dialogueManager.Dialogue(m_dialogueTexts).CancelWith(gameObject));
    }

    void ResumeTimeline()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_dialogueEnded);
        m_timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }

    void PauseTimeline()
    {
        m_timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }


    void TimelinePlayed(PlayableDirector obj)
    {
        GameManager.instance.ChangeCanMove(false);
    }

    void TimelineStopped(PlayableDirector obj)
    {
        GameManager.instance.ChangeCanMove(true);
    }
}
