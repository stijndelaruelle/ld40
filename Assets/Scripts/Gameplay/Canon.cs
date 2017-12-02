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
    private ObjectPool m_BulletPool;

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

        //Fire bullet
        Bullet bullet = (Bullet)m_BulletPool.ActivateAvailableObject();
        bullet.StartFlying(transform.position + (transform.forward * (transform.localScale.z * 0.5f)), transform.forward);

        //Set cooldown
        m_CooldownTimer = m_Cooldown;
    }
}
