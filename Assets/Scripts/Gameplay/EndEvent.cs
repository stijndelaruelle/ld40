using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEvent : MonoBehaviour
{
    public event Action LevelEndEvent;

    public void OnTriggerEnter(Collider other)
    {
        Ship ship = other.GetComponent<Ship>();
        if (ship != null)
        {
            ship.OnVictory();

            if (LevelEndEvent != null)
                LevelEndEvent();
        }
    }
}
