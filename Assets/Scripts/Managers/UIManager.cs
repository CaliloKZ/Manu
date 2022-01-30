using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Image img;

    [SerializeField]
    private Slider m_bookshelfMinigameLeftSlider,
                   m_bookshelfMinigameRightSlider;

    [SerializeField]
    private List<GameObject> m_bookshelfTexts = new List<GameObject>();

    [SerializeField]
    private GameObject m_newItemPanel,
                       m_documentPanel;

    [Header("Color of the dialogue outline")][SerializeField]
    private Color m_manuelaFontColor;
    [SerializeField]
    private Color m_manuelFontColor,
                  m_momFontColor,
                  m_vendorFontColor,
                  m_alleyManFontColor;

    [Header("Dialogue Voices")]
    [SerializeField]
    private List<string> m_manuelaVoices = new List<string>();
    [SerializeField]
    private List<string> m_manuelVoices = new List<string>();
    [SerializeField]
    private List<string> m_momVoices = new List<string>();
    [SerializeField]
    private List<string> m_breadVoices = new List<string>();
    [SerializeField]
    private List<string> m_alleyVoices = new List<string>();

    #region getColorsAndVoices

    public Color GetColor(int character)// 0 = manuela, 1 = manuel, 2 = mom, 3 = bread, 4 = alley;
    {
        switch(character)
        {
            case 0:
                return m_manuelaFontColor;
            case 1:
                return m_manuelFontColor;
            case 2:
                return m_momFontColor;
            case 3:
                return m_vendorFontColor;
            case 4:
                return m_alleyManFontColor;
            default:
                return Color.white;
        }
    }

    public List<string> GetVoice(int character)// 0 = manuela, 1 = manuel, 2 = mom, 3 = bread, 4 = alley;
    {
        switch (character)
        {
            case 0:
                return m_manuelaVoices;
            case 1:
                return m_manuelVoices;
            case 2:
                return m_momVoices;
            case 3:
                return m_breadVoices;
            case 4:
                return m_alleyVoices;
            default:
                return null;
        }
    }
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of UIManager was found. Destroying gameObject");
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Timing.RunCoroutine(FadeImage(2).CancelWith(gameObject));
    }

    public void ChangeRoom(float seconds)
    {
        Timing.RunCoroutine(FadeImage(seconds).CancelWith(gameObject));
    }
    public IEnumerator<float> FadeInWithDelay(float seconds, float delay)
    {
        yield return Timing.WaitForSeconds(delay);
        Timing.RunCoroutine(FadeToImage(seconds).CancelWith(gameObject));
    }

    public IEnumerator<float> FadeInOut(float seconds)
    {
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(FadeToImage(seconds).CancelWith(gameObject)));
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(FadeImage(seconds).CancelWith(gameObject)));
    }

    public void ActivateBookshelfTexts(bool activate)
    {
        if (activate)
        {
            for (int i = 0; i < m_bookshelfTexts.Count; i++)
            {
                m_bookshelfTexts[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < m_bookshelfTexts.Count; i++)
            {
                m_bookshelfTexts[i].SetActive(false);
            }
        }
        
    }
    IEnumerator<float> FadeToImage(float seconds)
    {
        img.color = new Color(0, 0, 0, 1);
        for (float i = 0; i <= seconds; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return Timing.WaitForOneFrame;
        }
    }

    IEnumerator<float> FadeImage(float seconds)
    {
        img.color = new Color(0, 0, 0, 1);
        for (float i = seconds; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return Timing.WaitForOneFrame;
        }
    }

    public void SetBookshelfSliderMaximumValue(float value)
    {
        m_bookshelfMinigameLeftSlider.maxValue = value;
        m_bookshelfMinigameRightSlider.maxValue = value;
    }

    public void UpdateBookshelfSlider(bool isLeft, float value)
    {
        if(isLeft)
        {
            m_bookshelfMinigameLeftSlider.value = value;
        }
        else
        {
            m_bookshelfMinigameRightSlider.value = value;
        }
    }

    public void NewItem(Item item)
    {
        GameManager.instance.ChangeCanMove(false);
        m_newItemPanel.SetActive(true);
        m_newItemPanel.GetComponent<NewItemPanel>().NewItem(item);     
    }

    public void ActivateDocumentPanel()
    {
        GameManager.instance.ChangeCanMove(false);
        m_documentPanel.SetActive(true);
    }
}
