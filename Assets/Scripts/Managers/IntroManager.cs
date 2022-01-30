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
        m_continueBT.interactable = !string.IsNullOrWhiteSpace(PlayerPrefs.GetString("LastScene"));
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

}
