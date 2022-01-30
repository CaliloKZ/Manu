using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    private string m_SceneToLoad;
    [SerializeField]
    private Button m_continueBT;
    private void Start()
    {
        SoundManager.instance.PlayMusic("MenuPrincipal");
        if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("LastScene")))
        {
            m_continueBT.interactable = false;
        }
        else if(PlayerPrefs.GetString("LastScene") == "ElaHouseScene" || PlayerPrefs.GetString("LastScene") == "Credits")
        {
            m_continueBT.interactable = false;
        }
        else
        {
            m_continueBT.interactable = true; 
        }


    }

    public void PlayBT()
    {
        SoundManager.instance.StopMusic();
        SceneManager.LoadScene(m_SceneToLoad);
    }

    public void ContinueBT()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("LastScene"));
    }

    public void OptionsBT()
    {

    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
        SoundManager.instance.PlayMusic("FinalCutscene");
    }

}
