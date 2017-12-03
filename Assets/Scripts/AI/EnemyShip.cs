using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyShip : IDamagable
{
    public enum TargettedSide
    {
        NONE,
        LEFT,
        RIGHT
    }
    public TargettedSide TargetSide = TargettedSide.NONE;

    [SerializeField]
    private Transform m_Player;
    private Vector3 m_LeftsidePlayer, m_RightsidePlayer, m_Target;
    private NavMeshAgent m_Agent;

    [SerializeField]
    private GameObject[] m_Loot;

    public EnemySpawner SpawnParent
    {
        set;
        private get;
    }

    [SerializeField]
    private float m_ShootPerSecond;
    private float m_ShootCooldown;

    private List<Canon> m_Canons;

    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        m_Canons = new List<Canon>();
        m_Canons.AddRange(GetComponentsInChildren<Canon>());
        Spawn();

        OnSinkEvent += OnSink;
    }

    public void SetPlayer(GameObject _player)
    {
        Debug.Log("Setting player");
        m_Player = _player.transform;
    }

    public void Spawn()
    {
        Debug.Log("Spawning enemy");
        transform.position = new Vector3(Random.Range(-120f, 120f), 0, Random.Range(-120f, 120f)) + m_Player.transform.position;
    }

    private void Update()
    {
        if (IsSunk)
            return;

        if (!m_Player)
            return;

        if (m_Player.GetComponent<Ship>().GameEnded)
        {
            Sink();
            return;
        }

        if (Vector3.Distance(transform.position, m_Player.transform.position) < 30)
        {
            if (Vector3.Dot(transform.right, m_Player.right) > 0)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(m_Player.forward), 10 * Time.deltaTime);
            else
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-m_Player.forward), 10 * Time.deltaTime);

            m_ShootCooldown += Time.deltaTime;
            if (m_ShootCooldown >= m_ShootPerSecond && Vector3.Distance(transform.position, m_Player.transform.position) <= 17)
            {
                m_ShootCooldown = 0;
                foreach (Canon _canon in m_Canons)
                {
                    if (Vector3.Dot(_canon.transform.forward, m_Player.transform.position) < 0)
                    {
                        _canon.Fire();
                    }
                }
            }
        }

        m_LeftsidePlayer = m_Player.position + (m_Player.right * 15f);
        m_RightsidePlayer = -m_Player.position - (m_Player.right * 15f);
    }

    private void OnSink()
    {
        //m_Agent.ResetPath();
        int _random = Random.Range(0, m_Loot.Length);
        GameObject _loot = Instantiate(m_Loot[_random], transform.position - (Vector3.up * 2), Quaternion.identity);

        Sequence _seq = DOTween.Sequence();
        _seq.Append(transform.DORotate(new Vector3(90, 0, 0), 2));
        _seq.Append(transform.DOMoveY(-5, 4));
        _seq.AppendInterval(10);
        _seq.OnComplete(Deactivate);
        _seq.Play();
    }

    public void ChangeDirection()
    {
        if (IsSunk)
            return;

        m_Target = GetNearest();
        m_Agent.SetDestination(m_Target);
    }

    IEnumerator SteerChange()
    {
        yield return new WaitForSeconds(3);
        while (!IsSunk)
        {
            if (m_Player)
            {
                m_Target = GetNearest();
                m_Agent.SetDestination(m_Target);
            }
            yield return new WaitForSeconds(3);
        }
        m_Agent.ResetPath();
    }

    private Vector3 GetNearest()
    {
        float _distance = Vector3.Distance(m_LeftsidePlayer, transform.position);
        int _side = 0;
        TargettedSide _old = TargetSide;
        if (Vector3.Distance(m_RightsidePlayer, transform.position) < _distance)
        {
            if (SpawnParent.SideUsed == TargettedSide.LEFT && _old == TargettedSide.NONE)
            {
                SpawnParent.SideUsed = TargettedSide.RIGHT;
                _side = 1;
            }
            else
            {
                SpawnParent.SideUsed = TargettedSide.LEFT;
                _side = -1;
            }
        }
        else
        {
            if (SpawnParent.SideUsed == TargettedSide.LEFT && _old == TargettedSide.NONE)
            {
                SpawnParent.SideUsed = TargettedSide.RIGHT;
                _side = 1;
            }
            else
            {
                SpawnParent.SideUsed = TargettedSide.LEFT;
                _side = -1;
            }
        }
        TargetSide = SpawnParent.SideUsed;
        if (_old != TargetSide)
            SpawnParent.AlertRedirection(this);

        if (_side == -1)
            return m_LeftsidePlayer;
        else
            return m_RightsidePlayer;

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
        if (m_Agent)
            StartCoroutine(SteerChange());
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        Reset();
        if (m_Agent)
            Start();
    }

    public override bool IsAvailable()
    {
        return (!gameObject.activeInHierarchy);
    }
    #endregion
}
