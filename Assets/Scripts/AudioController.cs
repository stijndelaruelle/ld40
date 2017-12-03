using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private bool m_IsMainBGM;

    [SerializeField]
    private float m_LoopFromSec;

    public AudioSource m_Intro, m_Main;

    private AudioSource m_Source;

    // Use this for initialization
    void Start()
    {
        m_Source = GetComponent<AudioSource>();
        m_Intro.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Intro.time >= m_LoopFromSec && !m_IsMainBGM)
        {
            m_IsMainBGM = true;
            m_Main.Play();
        }
    }
}
