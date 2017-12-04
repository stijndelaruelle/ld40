using DG.Tweening;
using Sjabloon;
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
    private PoolableObject m_Bullet;

    [Header("Effects")]
    [SerializeField]
    private List<ParticleSystem> m_Particles;
    [SerializeField]
    private AudioClip[] m_ShootSFX;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        //Debug
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Fire();
        //}

        HandleCooldownTimer();
        HandleDragging();
    }

    private void HandleCooldownTimer()
    {
        if (m_CooldownTimer > 0.0f)
            m_CooldownTimer -= Time.deltaTime;
    }

    private void HandleDragging()
    {
        if (!IsDragged)
            return;

        RaycastHit hitInfo;
        Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1000.0f);

        if (hitInfo.collider == null)
            return;

        //Super cheap.
        if (hitInfo.collider.gameObject.tag != "Player")
            return;

        Vector3 diff = transform.position - hitInfo.collider.transform.position;
        Vector3 right = hitInfo.collider.gameObject.transform.right;

        float dot = Vector3.Dot(right, diff);

        if (dot < 0)
        {
            transform.DOLocalRotate(new Vector3(0.0f, -90.0f, 0.0f), 0.5f);
        }
        else
        {
            transform.DOLocalRotate(new Vector3(0.0f, 90.0f, 0.0f), 0.5f);
        }
    }

    public void Fire()
    {
        Fire(null, null);
    }

    public void Fire(Mesh mesh, Material[] materials)
    {
        if (m_CooldownTimer > 0.0f)
            return;

        if (CanUse == false)
            return;

        //Fire bullet
        Bullet bullet = (Bullet)ObjectPoolManager.Instance.GetPool(m_Bullet).ActivateAvailableObject();
        bullet.StartFlying(m_FirePosition.position, m_FirePosition.forward, mesh, materials);

        //Set cooldown
        m_CooldownTimer = m_Cooldown;

        foreach (ParticleSystem particle in m_Particles)
        {
            particle.Play();
        }

        if (m_ShootSFX.Length > 0)
        {
            int _r = Random.Range(0, m_ShootSFX.Length);
            GetComponent<AudioSource>().PlayOneShot(m_ShootSFX[_r]);
        }
    }
}
