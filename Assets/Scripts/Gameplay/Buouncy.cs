using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Buouncy : MonoBehaviour
{
    private Vector3 m_StartPosition;

    private void Start()
    {
        m_StartPosition = transform.position;
    }

    private void Update()
    {
        transform.position = m_StartPosition + new Vector3(0, Mathf.Sin(Time.time * 2) / 5f, 0.0f);
    }
}