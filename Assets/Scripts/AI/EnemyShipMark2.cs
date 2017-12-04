using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShipMark2 : IDamagable
{
    [Header("Enemy")]
    [SerializeField]
    private float m_Range;

    [SerializeField]
    private Ship m_Player;
    private bool m_HasSeenPlayer = false;

    [SerializeField]
    private List<Canon> m_Canons;

    [Header("Path")]
    [SerializeField]
    private Transform m_TargetPosition;

    [SerializeField]
    private float m_Duration;

    [SerializeField]
    private Ease m_Ease = Ease.InOutQuad;

    private void Start()
    {
        OnSinkEvent += OnSink;
    }

    private void Update()
    {
        if (IsSunk)
            return;

        if (m_Player.IsSunk)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (Canon canon in m_Canons)
            {
                canon.Fire();
            }
        }

        HandleMovement();
        HandleShooting();
    }

    private void OnSink()
    {
        //int _random = Random.Range(0, m_Loot.Length);
        //GameObject _loot = Instantiate(m_Loot[_random], transform.position - (Vector3.up * 2), Quaternion.identity);

        //Let go our canons
        foreach(Canon canon in m_Canons)
        {
            canon.SetParent(null);
        }

        transform.DOKill();

        //Sink animation
        Sequence _seq = DOTween.Sequence();
        _seq.Append(transform.DORotate(new Vector3(-90, 0, 0), 1));
        _seq.Append(transform.DOMoveY(-5, 1));
        _seq.AppendInterval(10);
        _seq.OnComplete(Deactivate);
        _seq.Play();
    }

    private void HandleMovement()
    {
        if (m_HasSeenPlayer)
            return;

        Vector3 diff = transform.position - m_Player.transform.position;
        float distance = diff.magnitude;

        if (distance < m_Range)
        {
            m_HasSeenPlayer = true;

            //Start moving
            transform.DOMove(m_TargetPosition.position, m_Duration).SetEase(m_Ease);
        }
    }

    private void HandleShooting()
    {
        if (m_HasSeenPlayer == false)
            return;

        //Fire away enlessly!
        foreach (Canon canon in m_Canons)
        {
            canon.Fire();
        }
    }

    #region PoolableObject
    public override void Initialize()
    {

    }

    public override void Activate()
    {

    }

    public override void Deactivate()
    {

    }

    public override bool IsAvailable()
    {
        return (!gameObject.activeInHierarchy);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_Range);
    }
}
