using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Image img;

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

    public void ChangeRoom()
    {
        Timing.RunCoroutine(FadeImage(1).CancelWith(gameObject));
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
}
