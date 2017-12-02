using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDamagable : PoolableObject
{
    [SerializeField]
    private int m_Health;
    public int GetHealth
    {
        get { return m_Health; }
    }

    [SerializeField]
    private bool m_IsSunk;
    public bool IsSunk
    {
        get { return m_IsSunk; }
    }

    [SerializeField]
    public bool Indestructible;

    public void Damage(int _damage)
    {
        m_Health -= _damage;
        if (m_Health <= 0)
        {
            m_Health = 0;
            m_IsSunk = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsSunk)
        {
            if (collision.transform.GetComponent<Bullet>() || collision.transform.GetComponent<IDamagable>())
            {
                Damage(1);
            }
        }
        else if (Indestructible)
        {
            if (collision.gameObject.GetComponent<IDamagable>())
            {
                collision.gameObject.GetComponent<IDamagable>().Damage(100);
            }
        }
    }
}
