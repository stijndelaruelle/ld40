using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolableObject
{
    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private float m_Gravity;
    private Vector3 m_Direction;

    public void StartFlying(Vector3 postion, Vector3 direction)
    {
        transform.position = postion;
        m_Direction = direction;
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        //Move
        Vector3 deltaPosition = m_Direction * m_Speed * Time.deltaTime;
        m_Direction.y -= m_Gravity * Time.deltaTime;

        transform.position += deltaPosition;

        //Rotate (fix: klopt niet helemaal)
        transform.rotation = Quaternion.LookRotation(m_Direction);
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
