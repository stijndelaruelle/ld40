using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    [SerializeField]
    private ImageFader m_ImageFader;
    public void Retry()
    {
        m_ImageFader.FadeIn(OnFadeInComplete);
    }

    private void OnFadeInComplete()
    {
        SceneManager.LoadScene("main");
    }
}
