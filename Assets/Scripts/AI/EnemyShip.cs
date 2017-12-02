using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShip : MonoBehaviour
{
    [SerializeField]
    private Transform m_Player;
    [SerializeField]
    private Vector3 m_LeftsidePlayer, m_RightsidePlayer, m_Target;
    private NavMeshAgent m_Agent;

    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_Agent = GetComponent<NavMeshAgent>();
        m_LeftsidePlayer = m_Player.right * 20f;
        m_RightsidePlayer = -m_Player.right * 20f;

        m_Target = GetNearest();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, m_Player.transform.position) < 30)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(m_Player.forward), 5 * Time.deltaTime);
        }
        else
        {
            m_Agent.SetDestination(m_Target);
        }
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
}
