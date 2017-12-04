using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_OnHover;
    [SerializeField]
    private AudioClip m_OnClick;

    private AudioSource m_Source;

    public void Start()
    {
        m_Source = GetComponent<AudioSource>();
    }

    public void OnHover()
    {
        m_Source.PlayOneShot(m_OnHover);
    }

    public void OnClick()
    {
        m_Source.PlayOneShot(m_OnClick);
    }
}
