using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private Animator[] m_Animators;

    public void StartGameOver()
    {
        foreach (Animator _anim in m_Animators)
        {
            _anim.SetBool("IsGameOver", true);
        }
    }

    public void StartGame()
    {
        Retry();
    }

    public void Retry()
    {
        SceneManager.LoadScene("main");
    }

    public void Quit()
    {
        Application.Quit();
    }
}