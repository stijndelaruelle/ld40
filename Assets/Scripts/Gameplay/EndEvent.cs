using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEvent : MonoBehaviour
{

    public UIHandler m_InterfaceHandler;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ship>())
        {
            other.GetComponent<Ship>().GameEnded = true;
            m_InterfaceHandler.StartGameEnd();
        }
    }
}
