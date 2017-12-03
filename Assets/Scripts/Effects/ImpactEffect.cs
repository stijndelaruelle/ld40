using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : PoolableObject
{
    [SerializeField]
    private float m_LifeTime;

    private ParticleSystem[] m_ParticleSystems;

    public void Play(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        transform.DOKill();

        gameObject.SetActive(true);

        for (int i = 0; i < m_ParticleSystems.Length; ++i)
        {
            m_ParticleSystems[i].Play();

            //Deactivate in x sec
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(m_LifeTime);
            sequence.AppendCallback(Deactivate);

            sequence.Play();
        }
    }

    #region PoolableObject
    public override void Initialize()
    {
        m_ParticleSystems = transform.GetComponentsInChildren<ParticleSystem>();
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public override bool IsAvailable()
    {
        return (!gameObject.activeInHierarchy);
    }
    #endregion
}
