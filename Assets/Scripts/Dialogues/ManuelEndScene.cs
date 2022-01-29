using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using MEC;

public class ManuelEndScene : MonoBehaviour
{

    private DialogueManager m_dialogueManager;

    [SerializeField]
    private List<DialogueText> m_closeDocumentDialogue = new List<DialogueText>();
    [SerializeField]
    private List<DialogueText> m_findPhotoDialogue = new List<DialogueText>();

    private UnityAction m_endDialogue;

    [SerializeField]
    private Item m_photo;

    private PlayableDirector m_timeline;

    private void Awake()
    {
        m_timeline = GetComponent<PlayableDirector>();
        m_timeline.stopped += TimelineEnded;
    }
    private void Start()
    {
        m_dialogueManager = DialogueManager.instance;
    }

    public void ClosedDocument()
    {
        m_endDialogue += FindPhoto;
        m_dialogueManager.dialogueEnded.AddListener(m_endDialogue);
        m_dialogueManager.DialogueState(true);
        Timing.RunCoroutine(m_dialogueManager.Dialogue(m_closeDocumentDialogue).CancelWith(gameObject));
    }

    void FindPhoto()
    {
        m_dialogueManager.dialogueEnded.RemoveListener(m_endDialogue);
        UIManager.instance.NewItem(m_photo);
    }

    public void ClosedPhoto()
    {
        m_endDialogue -= FindPhoto;
        m_endDialogue += EndScene;
        m_dialogueManager.dialogueEnded.AddListener(m_endDialogue);
        m_dialogueManager.DialogueState(true);
        Timing.RunCoroutine(m_dialogueManager.Dialogue(m_findPhotoDialogue).CancelWith(gameObject));
    }

    void EndScene()
    {
        GameManager.instance.ChangeCanMove(false);
        m_dialogueManager.dialogueEnded.RemoveListener(m_endDialogue);
        m_timeline.Play();
    }

    void TimelineEnded(PlayableDirector obj)
    {
        GameManager.instance.GoToCredits();
    }
}
