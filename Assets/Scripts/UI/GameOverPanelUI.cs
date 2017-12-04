using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanelUI : MonoBehaviour
{
    [SerializeField]
    private Ship m_Ship;

    [SerializeField]
    private List<AnimationStepChain> m_Animations;

    private void Start()
    {
        m_Ship.DeathEvent += OnShipDeath;
    }

    private void OnShipDeath()
    {
        foreach(AnimationStepChain animation in m_Animations)
        {
            animation.Play();
        }
    }
}
