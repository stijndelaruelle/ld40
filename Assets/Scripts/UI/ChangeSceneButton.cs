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

    [SerializeField]
    private AudioFader m_AudioFader;

    public void Retry()
    {
        m_ImageFader.FadeIn(OnFadeInComplete);
        m_AudioFader.FadeOut();
    }

    private void OnFadeInComplete()
    {
        SceneManager.LoadScene(m_SceneName);
    }
}
