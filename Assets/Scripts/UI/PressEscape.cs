using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEscape : MonoBehaviour
{
    [SerializeField]
    private ImageFader m_ImageFader;

    [SerializeField]
    private AudioFader m_AudioFader;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToMainMenu();
        }
    }

    public void ToMainMenu()
    {
        m_ImageFader.FadeIn(OnFadeInComplete);
        m_AudioFader.FadeOut();
    }

    private void OnFadeInComplete()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
