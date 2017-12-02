using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : ICargo
{
    [Header("Canon")]
    [SerializeField]
    private float m_Cooldown;
    private float m_CooldownTimer;

    [SerializeField]
    private Transform m_FirePosition;

    [SerializeField]
    private ObjectPool m_BulletPool;

    [SerializeField]
    private List<ParticleSystem> m_Particles;

    private void Start()
    {
        m_BulletPool = GameObject.Find("BulletPool").GetComponent<ObjectPool>();
    }

    protected override void Update()
    {
        base.Update();

        //Debug
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        HandleCooldownTimer();
    }

    private void HandleCooldownTimer()
    {
        if (m_CooldownTimer > 0.0f)
            m_CooldownTimer -= Time.deltaTime;
    }

    public void Fire()
    {
        if (m_CooldownTimer > 0.0f)
            return;

        if (CanUse)
            return;

        //Fire bullet
        Bullet bullet = (Bullet)m_BulletPool.ActivateAvailableObject();
        bullet.StartFlying(m_FirePosition.position, transform.forward);

        //Set cooldown
        m_CooldownTimer = m_Cooldown;

        foreach(ParticleSystem particle in m_Particles)
        {
            particle.Play();
        }
    }
}
