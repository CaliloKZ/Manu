using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    private string m_SceneToLoad;

    public void PlayBT()
    {
        SceneManager.LoadScene(m_SceneToLoad);
    }

    public void OptionsBT()
    {

    }

    public void ExitBT()
    {
        Application.Quit();
    }
}
