using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanelUI : MonoBehaviour
{
    [SerializeField]
    private EndEvent m_EndEvent;

    [SerializeField]
    private AudioSource m_AudioSource;

    [SerializeField]
    private List<AnimationStepChain> m_Animations;
    private bool m_Done = false;

    private void Start()
    {
        m_EndEvent.LevelEndEvent += OnLevelEnd;
    }

    private void OnLevelEnd()
    {
        if (m_Done)
            return;

        //Play sound
        m_AudioSource.Play();

        //Trigger animation
        foreach (AnimationStepChain animation in m_Animations)
        {
            animation.Play();
        }
        m_Done = true;
    }
}
