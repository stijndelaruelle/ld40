using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : ICargo
{
    [SerializeField]
    private float m_Cooldown;
    private float m_CooldownTimer;

    [Header("Required references")]
    [SerializeField]
    private ObjectPool m_BulletPool;

    private void Update()
    {
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
