using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pool reference")]
    [SerializeField]
    private ObjectPool m_EnemyPool;

    [Header("Spawn interval")]
    [SerializeField]
    private float m_SpawnInterval;

    private float m_Time;

    private void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time >= m_SpawnInterval)
        {
            m_Time = 0;
            EnemyShip _enemy = (EnemyShip)m_EnemyPool.ActivateAvailableObject();
        }
    }
}
