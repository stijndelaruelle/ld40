using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StearingWheel : MonoBehaviour
{
    [SerializeField]
    private float m_RotationSpeed;

    [SerializeField]
    private Ship m_Ship;

    private void Update()
    {
        transform.Rotate(0.0f, 0.0f, m_RotationSpeed * Time.deltaTime);
    }
}
