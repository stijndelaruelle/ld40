using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPanelUI : MonoBehaviour
{
    [SerializeField]
    private EndEvent m_EndEvent;

    [SerializeField]
    private Ship m_Ship;

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

        foreach(AnimationStepChain animation in m_Animations)
        {
            animation.Play();
        }
        m_Done = true;
    }
}
