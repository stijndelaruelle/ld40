using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnKeyPress : MonoBehaviour
{
    [SerializeField]
    private KeyCode m_KeyCode;

    [SerializeField]
    private string m_SceneName;

    [SerializeField]
    private ImageFader m_ImageFader;

    [SerializeField]
    private AudioFader m_AudioFader;

    private void Update()
    {
        if (Input.GetKeyDown(m_KeyCode))
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
        SceneManager.LoadScene(m_SceneName);
    }
}
