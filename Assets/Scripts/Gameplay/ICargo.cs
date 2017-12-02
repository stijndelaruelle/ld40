using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICargo : MonoBehaviour
{
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_Position; //-1 to 1
    public float Position
    {
        get { return m_Position; }
    }

    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float m_Weight;
    public float Weight
    {
        get { return m_Weight; }
    }
}
