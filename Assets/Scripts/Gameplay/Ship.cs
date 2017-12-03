﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ship : IDamagable
{
    [Header("Speed")]
    [SerializeField]
    private float m_MaxSpeed;
    private float m_CurrentSpeed;

    [SerializeField]
    private float m_WeightToSpeedRatio; //F.e.: if this is 5, it means: 5kg = 1 less speed

    [Header("Turning")]
    [SerializeField]
    private float m_MaxTiltAngle; //Higher than this and the ship sinks
    private Vector2 m_CurrentDirection;
    private float m_CummulativeAngle;
    private float m_CurrentAngle;

    [SerializeField]
    private float m_WeightToAngleRatio; //F.e.: if this is 5, it means:  5kg = add 1 degree per second

    [SerializeField]
    private AnimationCurve m_RotationAnimationCurve;

    [Header("Weight")]
    [SerializeField]
    private float m_MaxWeight; //Higher than this and the ship sinks

    [SerializeField]
    private float m_MaxDepth; //The transform the the deepest the boat can be
    private float m_DefaultDepth;

    [Header("Effects")]
    [SerializeField]
    private ParticleSystem[] m_WaterTrails;
    [SerializeField]
    private UIHandler m_UI;

    [Header("References")]
    [SerializeField]
    private BoxCollider m_BoxCollider;

    [HideInInspector]
    public bool GameEnded = false;

    [SerializeField]
    private List<ICargo> m_Cargo;
    private float m_CummulativeWeight = 0.0f;
    private float m_RelativeWeight = 0.0f;

    public List<ICargo> GetCargo
    {
        get { return m_Cargo; }
    }

    private void Start()
    {
        m_DefaultDepth = transform.position.y;
        m_CurrentSpeed = m_MaxSpeed;
        m_CurrentDirection = new Vector2(transform.forward.x, transform.forward.z);

        foreach (ICargo cargo in m_Cargo)
        {
            cargo.StartDragEvent += OnCargoStartDrag;
            cargo.DestroyEvent += OnCargoDestroy;
        }

        OnSinkEvent += OnGameOver;
        OnDamageEvent += OnDamage;

    }

    private void Update()
    {
        if (!IsSunk)
        {
            //Change movement
            Vector2 addedDirection = Vector2.zero;
            addedDirection = addedDirection.DegreeToVector2(m_CummulativeAngle);
            addedDirection.Normalize();

            m_CurrentAngle = Mathf.Atan2(addedDirection.y, addedDirection.x) * Mathf.Rad2Deg;

            m_CurrentDirection.x += addedDirection.y * Time.deltaTime;
            m_CurrentDirection.y += addedDirection.x * Time.deltaTime;

            m_CurrentDirection.Normalize();

            //Actually move
            Vector2 addedPosition = (m_CurrentSpeed * m_CurrentDirection) * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + addedPosition.x,
                                             transform.position.y,
                                             transform.position.z + addedPosition.y);
        }
    }

    private void OnDamage()
    {
        //Loot_Gold _eject = (Loot_Gold)m_Cargo.FirstOrDefault(c => c is Loot_Gold);
        //_eject.GetComponent<Rigidbody>().AddExplosionForce(10, _eject.transform.position, 1f);
        //m_Cargo.Remove(_eject);
    }

    private void OnGameOver()
    {
        //Drop all loot
        for (int i = m_Cargo.Count - 1; i >= 0; --i)
        {
            m_Cargo[i].SetParent(null);
            RemoveCargo(m_Cargo[i]);
        }

        foreach (ParticleSystem _fx in m_WaterTrails)
            _fx.Stop();

        Sequence _Seq = DOTween.Sequence();
        if (m_RelativeWeight > 0.3f || m_RelativeWeight < -0.3f)
        {
            float zAdd = 180 - transform.rotation.eulerAngles.z;

            _Seq.Append(transform.DORotate(new Vector3(0.0f, 0.0f, zAdd), 2, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine));
            _Seq.Insert(0, transform.DOMoveY(1, 1));
        }
        _Seq.Append(transform.DOMoveY(-5, 4));
        _Seq.OnComplete(m_UI.StartGameOver);
        _Seq.Play();

    }

    private void RecalculateSpeed()
    {
        float lostSpeed = 0;
        if (m_WeightToSpeedRatio > 0) { lostSpeed = m_CummulativeWeight / m_WeightToSpeedRatio; };

        m_CurrentSpeed = m_MaxSpeed - lostSpeed;
        if (m_CurrentSpeed < 0.0f)
            m_CurrentSpeed = 0.0f;

        foreach (ParticleSystem _fx in m_WaterTrails)
        {
            ParticleSystem.EmissionModule _emmission = _fx.emission;
            _emmission.rateOverTimeMultiplier = Mathf.InverseLerp(0, 10, m_CurrentSpeed) * 10;
        }
    }

    private void RecalculateAngle()
    {
        //Calculate angle
        m_CummulativeAngle = 0;
        if (m_WeightToAngleRatio > 0) { m_CummulativeAngle = m_RelativeWeight / m_WeightToAngleRatio; }

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, m_CurrentAngle, -m_CummulativeAngle));

        Tweener tweener = transform.DORotateQuaternion(targetRotation, 1.0f);
        tweener.SetEase(m_RotationAnimationCurve);
    }

    public void AddCargo(ICargo cargo)
    {
        if (m_Cargo.Contains(cargo))
            return;

        if (IsSunk)
            return;

        //Attach cargo to ship
        NormalizeCargoPosition(cargo);

        //cargo.gameObject.transform.parent = transform;
        cargo.StartDragEvent += OnCargoStartDrag;
        cargo.DestroyEvent += OnCargoDestroy;

        if (cargo.GetType() == typeof(LootOld))
        {
            if (!HasLoot())
                m_Cargo.Add(cargo);
        }
        else
            m_Cargo.Add(cargo);

        m_CummulativeWeight += cargo.Weight;
        m_RelativeWeight += cargo.Position * cargo.Weight;

        RecalculateSpeed();
        RecalculateAngle();
        CheckWeight();
    }

    private void CheckWeight()
    {
        float _weight = 0;
        foreach (ICargo _cargo in m_Cargo)
            _weight += _cargo.Weight;

        //Check if we die from going overweight
        if (_weight > m_MaxWeight)
        {
            Sink();
        }
        else
        {
            float diff = _weight / m_MaxWeight;

            float lerp = Mathf.Lerp(m_DefaultDepth, m_MaxDepth, diff);

            Tweener tweener = transform.DOMoveY(lerp, 1.0f);
            tweener.SetEase(m_RotationAnimationCurve);
        }

        //Check if we die from tilting too much to one side
        if (Mathf.Abs(m_CurrentAngle) > m_MaxTiltAngle)
        {
            Sink();
        }
    }

    public void RemoveCargo(ICargo cargo)
    {
        if (IsSunk)
            return;

        //Detach cargo from ship
        //cargo.gameObject.transform.parent = null;

        //Stop listening for events
        cargo.StartDragEvent -= OnCargoStartDrag;
        cargo.DestroyEvent -= OnCargoDestroy;

        //Remove from list
        m_Cargo.Remove(cargo);

        m_CummulativeWeight -= cargo.Weight;
        m_RelativeWeight -= cargo.Position * cargo.Weight;

        RecalculateSpeed();
        RecalculateAngle();
        CheckWeight();
    }

    public bool HasLoot()
    {
        return m_Cargo.OfType<LootOld>().Any();
    }

    private void NormalizeCargoPosition(ICargo cargo)
    {
        //Terrible calculation, sorry.

        //Calculate distance from forward
        Vector3 sameY = new Vector3(cargo.transform.position.x, transform.position.y, cargo.transform.position.z); //eliminate Y from the equation
        Vector3 diff = (sameY - transform.position); //Diff between cargo & ship point
        float angle = Vector3.Angle(diff, transform.forward); //Angle between that vector & the forward vector
        float distanceS = diff.magnitude; //Distances of the diff

        float distanceO = Mathf.Sin(angle * Mathf.Deg2Rad) * distanceS; //SOS
                                                                        //Debug.Log(distanceO);

        //Left or right?
        float dot = Vector3.Dot(transform.right, diff);
        distanceO *= Mathf.Sign(dot);

        //Debug.Log(dot);

        //Convert to -1, 1
        float totalWidth = m_BoxCollider.size.x * transform.localScale.x;
        float halfWidth = totalWidth * 0.5f;

        float remapped = distanceO.Remap(-halfWidth, halfWidth, -1.0f, 1.0f);
        remapped = Mathf.Clamp(remapped, -1.0f, 1.0f); //Can go outside because of an offset pivot.

        //Debug.Log(remapped);
        cargo.Position = remapped;
    }

    //Unity callbacks
    public void OnCollisionEnter(Collision collision)
    {
        //ICargo cargo = collision.collider.GetComponent<ICargo>();

        //if (cargo == null)
        //    return;

        //if (cargo.IsDragged)
        //    return;

        //AddCargo(cargo);
    }

    private void OnDrawGizmos()
    {
        Vector3 lineEnd = transform.position;
        lineEnd.x += m_CurrentDirection.x * 2.0f; //*2 just so we can see it
        lineEnd.z += m_CurrentDirection.y * 2.0f;

        Gizmos.DrawLine(transform.position, lineEnd);
    }

    //Custom callbacks
    private void OnCargoStartDrag(ICargo cargo)
    {
        RemoveCargo(cargo);
    }

    private void OnCargoDestroy(ICargo cargo)
    {
        RemoveCargo(cargo);
    }

    //Poolable object
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
        return true;
    }
}
