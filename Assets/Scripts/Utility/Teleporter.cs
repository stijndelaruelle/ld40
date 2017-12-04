using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Transform m_TeleportLocation;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = m_TeleportLocation.position;    
    }
}
