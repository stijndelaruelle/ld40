using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_Rotation;

    private void Update()
    {
        transform.Rotate(m_Rotation.x * Time.deltaTime,
                         m_Rotation.y * Time.deltaTime,
                         m_Rotation.z * Time.deltaTime);
    }
}
