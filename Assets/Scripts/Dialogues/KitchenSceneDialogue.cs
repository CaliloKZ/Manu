using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MEC;
using UnityEngine.Playables;

public class KitchenSceneDialogue : MonoBehaviour
{
    [SerializeField]
    private List<DialogueText> m_dialogueLines = new List<DialogueText>();

    private DialogueManager m_dialogueManager;
    private GameManager m_gameManager;

    private PlayableDirector m_timeline;

    [SerializeField]
    private GameObject m_playerCam,
                       m_sceneCam;

    private UnityAction m_resumeTimeline;


    private void Awake()
    {
        m_timeline = GetComponent<PlayableDirector>();
        m_timeline.played += TimelinePlayed;
        m_timeline.stopped += TimelineStopped;
    }
    private void Start()
    {
        m_resumeTimeline += ResumeTimeline;
        m_dialogueManager = DialogueManager.instance;
        m_gameManager = GameManager.instance;
    }

    public void StartDialogue()
    {
        Timing.RunCoroutine(Dialogue().CancelWith(gameObject));
    }
   IEnumerator<float> Dialogue()
    {
        m_dialogueManager.dialogueEnded.AddListener(m_resumeTimeline);
        m_dialogueManager.DialogueState(true);
        Timing.RunCoroutine(m_dialogueManager.Dialogue(m_dialogueLines).CancelWith(gameObject));
        yield return Timing.WaitForSeconds(2f);
        m_timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    void ResumeTimeline()
    {
        m_gameManager.ChangeCanMove(false);
        m_timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
        m_dialogueManager.dialogueEnded.RemoveListener(m_resumeTimeline);
    }

    void TimelinePlayed(PlayableDirector obj)
    {
        m_gameManager.ChangeCanMove(false);
    }

    void TimelineStopped(PlayableDirector obj)
    {
        m_gameManager.KitchenSpySceneEnded();       
        m_playerCam.SetActive(true);
        m_sceneCam.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            m_timeline.Play();
            m_sceneCam.SetActive(true);
            m_playerCam.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
