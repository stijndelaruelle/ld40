using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pool reference")]
    [SerializeField]
    private PoolableObject m_Enemy;

    [Header("Spawn interval")]
    [SerializeField]
    private float m_SpawnInterval;

    [Header("Misc")]
    [SerializeField]
    private GameObject m_Player;

    private float m_Time;

    private List<EnemyShip> m_Ships = new List<EnemyShip>();
    private EnemyShip.TargettedSide m_SidesUsed;
    public EnemyShip.TargettedSide SideUsed
    {
        get { return m_SidesUsed; }
        set { m_SidesUsed = value; }
    }

    private void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time >= m_SpawnInterval)
        {
            m_Time = 0;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        EnemyShip _enemy = (EnemyShip)ObjectPoolManager.Instance.GetPool(m_Enemy).ActivateAvailableObject();
        _enemy.Spawn(transform.position, m_Player, this);

        if (!m_Ships.Contains(_enemy))
            m_Ships.Add(_enemy);
    }

    public void AlertRedirection(EnemyShip _instigator)
    {
        foreach (EnemyShip _ship in m_Ships)
        {
            if (!_ship.gameObject.activeSelf)
                return;

            if (_ship.Equals(_instigator))
                continue;

            if (_ship.IsSunk)
                continue;

            if (_ship.TargetSide == _instigator.TargetSide)
                _ship.ChangeDirection();
        }
    }
}
