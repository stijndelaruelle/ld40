﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private Animator[] m_Animators;

    public void StartGameOver()
    {
        foreach (Animator _anim in m_Animators)
            _anim.SetBool("IsGameOver", true);
    }
}
