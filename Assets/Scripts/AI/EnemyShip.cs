using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShip : IDamagable
{
    [SerializeField]
    private Transform m_Player;
    [SerializeField]
    private Vector3 m_LeftsidePlayer, m_RightsidePlayer, m_Target;
    private NavMeshAgent m_Agent;

    [SerializeField]
    private float m_ShootPerSecond;
    private float m_ShootCooldown;

    private List<Canon> m_Canons;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_Agent = GetComponent<NavMeshAgent>();
        m_LeftsidePlayer = m_Player.right * 20f;
        m_RightsidePlayer = -m_Player.right * 20f;

        m_Canons = new List<Canon>();
        m_Canons.AddRange(GetComponentsInChildren<Canon>());
        Spawn();
    }

    public void Spawn()
    {
        transform.position = new Vector3(Random.Range(-120f, 120f), 0, Random.Range(-120f, 120f)) + m_Player.transform.position;
        InvokeRepeating("SteerChange", 0, 2) ;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, m_Player.transform.position) < 30)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(m_Player.forward), 10 * Time.deltaTime);
            m_ShootCooldown += Time.deltaTime;
            if (m_ShootCooldown >= m_ShootPerSecond)
            {
                m_ShootCooldown = 0;
                foreach (Canon _canon in m_Canons)
                {
                    if (Vector3.Dot(_canon.transform.forward, m_Player.transform.position) > 0)
                    {
                        _canon.Fire();
                    }
                }
            }
        }
        else
        {
            m_Agent.SetDestination(m_Target);
        }
    }

    private void SteerChange()
    {
        m_Target = GetNearest();
    }

    private Vector3 GetNearest()
    {
        float _distance = Vector3.Distance(m_LeftsidePlayer, transform.position);
        if (Vector3.Distance(m_RightsidePlayer, transform.position) < _distance)
            return m_RightsidePlayer;
        else
            return m_LeftsidePlayer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_LeftsidePlayer, 0.5f);
        Gizmos.DrawSphere(m_RightsidePlayer, 0.5f);
    }

    #region PoolableObject
    public override void Initialize()
    {

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
