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

    [Header("Color of the dialogue outline")][SerializeField]
    private Color m_manuelaFontColor;
    [SerializeField]
    private Color m_manuelFontColor,
                  m_momFontColor,
                  m_vendorFontColor,
                  m_alleyManFontColor;
    #region getFontColors
    public Color GetManuelaColor()
    {
        return m_manuelaFontColor;
    }

    public Color GetManuelColor()
    {
        return m_manuelFontColor;
    }

    public Color GetVendorColor()
    {
        return m_vendorFontColor;
    }

    public Color GetAlleyManColor()
    {
        return m_alleyManFontColor;
    }

    public Color GetMomColor()
    {
        return m_momFontColor;
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
    }

    private void Start()
    {
        Timing.RunCoroutine(FadeImage(2).CancelWith(gameObject));
    }

    public void ChangeRoom(float seconds)
    {
        Timing.RunCoroutine(FadeImage(seconds).CancelWith(gameObject));
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
}
