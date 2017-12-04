using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_AudioSource;

    [SerializeField]
    private AudioClip m_Intro;

    [SerializeField]
    private AudioClip m_Main;

    private void Start()
    {
        m_AudioSource.clip = m_Intro;
        m_AudioSource.loop = false;
        m_AudioSource.Play();
    }

    private void Update()
    {
        if (m_AudioSource.isPlaying == false &&
            m_AudioSource.clip == m_Intro)
        {
            m_AudioSource.clip = m_Main;
            m_AudioSource.loop = true;
            m_AudioSource.Play();
        }
    }
}
