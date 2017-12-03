using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPosition : MonoBehaviour
{
    [SerializeField]
    private float m_MinY;

    private void LateUpdate()
    {
        if (transform.position.y < m_MinY)
        {
            transform.position = new Vector3(transform.position.x,
                                             m_MinY,
                                             transform.position.z);
        }
    }
}
