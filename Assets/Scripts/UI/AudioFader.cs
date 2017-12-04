using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioFader : MonoBehaviour
{
    [SerializeField]
    private AudioMixerSnapshot m_Original;

    [SerializeField]
    private AudioMixerSnapshot m_FadedOut;

    [SerializeField]
    private float m_FadeInSpeed = 1.0f;

    [SerializeField]
    private float m_FadeOutSpeed = 1.0f;

    [SerializeField]
    private bool m_FadeInOnAwake = true;

    private void Start()
    {
        if (m_FadeInOnAwake)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        m_Original.TransitionTo(m_FadeInSpeed);
    }

    public void FadeOut()
    {
        m_FadedOut.TransitionTo(m_FadeOutSpeed);
    }

}
