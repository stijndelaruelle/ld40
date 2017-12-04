using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField]
    private string m_SceneName;

    [SerializeField]
    private ImageFader m_ImageFader;
    public void Retry()
    {
        m_ImageFader.FadeIn(OnFadeInComplete);
    }

    private void OnFadeInComplete()
    {
        SceneManager.LoadScene(m_SceneName);
    }
}
