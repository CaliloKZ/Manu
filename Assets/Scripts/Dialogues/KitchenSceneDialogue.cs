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
    [TextArea] [SerializeField]
    private List<string> m_finishDialogues = new List<string>(); //temp

    private DialogueManager m_dialogueManager;

    private PlayableDirector m_timeline;

    [SerializeField]
    private GameObject m_playerCam,
                       m_sceneCam;


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
            yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(m_dialogueLines[i], UIManager.instance.GetMomColor()).CancelWith(gameObject)));
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
        GameManager.instance.KitchenSpySceneEnded();       
        m_playerCam.SetActive(true);
        m_sceneCam.SetActive(false);
        Timing.RunCoroutine(LastDialogues().CancelWith(gameObject)); //temp
    }

    IEnumerator<float> LastDialogues() //temp
    {
        for (int i = 0; i < m_finishDialogues.Count; i++)
        {
            yield return Timing.WaitUntilDone(Timing.RunCoroutine(m_dialogueManager.Dialogue(m_finishDialogues[i], UIManager.instance.GetManuelaColor()).CancelWith(gameObject)));
            yield return Timing.WaitForSeconds(2f);
        }
        m_dialogueManager.StopDialogue();
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
