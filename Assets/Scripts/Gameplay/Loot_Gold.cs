using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot_Gold : ICargo
{
    [Header("Loot")]
    [SerializeField]
    private float m_Value;

    public float GetValue
    {
        get { return m_Value; }
    }
}
