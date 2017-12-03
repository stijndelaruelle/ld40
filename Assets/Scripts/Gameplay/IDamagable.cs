using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DamageDelegate();
public delegate void SinkDelegate();

public abstract class IDamagable : PoolableObject
{
    public event DamageDelegate OnDamageEvent;
    public event SinkDelegate OnSinkEvent;

    public AudioSource m_BubbleSFX;
    public ImpactEffect m_BubblePS;

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

    public void Reset()
    {
        m_Health = 1;
        m_IsSunk = false;
    }

    public void Damage(int _damage)
    {
        m_Health -= _damage;
        if (m_Health <= 0)
        {
            m_Health = 0;
            Sink();
        }
        if (OnDamageEvent != null)
            OnDamageEvent();
    }

    public void Sink()
    {
        m_IsSunk = true;
        if (OnSinkEvent != null)
            OnSinkEvent();

        ImpactEffect bubblesFx = (ImpactEffect)ObjectPoolManager.Instance.GetPool(m_BubblePS).ActivateAvailableObject();
        bubblesFx.Play(transform.position, Quaternion.identity);

        if (m_BubbleSFX)
            m_BubbleSFX.PlayOneShot(m_BubbleSFX.clip);
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
