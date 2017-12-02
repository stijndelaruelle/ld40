using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : ICargo
{
    [Header("General settings")]
    [SerializeField]
    private int m_State;

    private void Start()
    {
        EndDragEvent += ChangeState;
    }

    void ChangeState(ICargo _target)
    {

    }

    public void MakeBigger()
    {
        m_State++;
        // TODO: change visuals
    }
}
