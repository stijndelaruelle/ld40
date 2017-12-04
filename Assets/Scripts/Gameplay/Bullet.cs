using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolableObject
{
    [SerializeField]
    private MeshFilter m_MeshFilter;
    private Mesh m_DefaultMesh;

    [SerializeField]
    private MeshRenderer m_MeshRenderer;
    private Material[] m_DefaultMaterials;

    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private float m_Gravity;
    private Vector3 m_Direction;

    [Header("Effects")]
    [SerializeField]
    private PoolableObject m_ImpactExplosion;

    [SerializeField]
    private PoolableObject m_ImpactWater;

    public void StartFlying(Vector3 postion, Vector3 direction, Mesh mesh, Material[] materials)
    {
        transform.position = postion;
        m_Direction = direction;

        if (mesh != null)
        {
            m_MeshFilter.mesh = mesh;
            m_MeshRenderer.materials = materials;
        }
        else
        {
            m_MeshFilter.mesh = m_DefaultMesh;
            m_MeshRenderer.materials = m_DefaultMaterials;
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        ImpactEffect effect = null;

        if (collision.collider.gameObject.tag == "Water")
        {
            //Water effect
            ObjectPool pool = ObjectPoolManager.Instance.GetPool(m_ImpactWater);
            effect = (ImpactEffect)pool.ActivateAvailableObject();
        }
        else
        {
            //Other effect
            ObjectPool pool = ObjectPoolManager.Instance.GetPool(m_ImpactExplosion);
            effect = (ImpactEffect)pool.ActivateAvailableObject();
        }

        if (effect != null)
            effect.Play(transform.position, Quaternion.identity);

        Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        ImpactEffect effect = null;

        if (other.gameObject.tag == "Water")
        {
            //Water effect
            ObjectPool pool = ObjectPoolManager.Instance.GetPool(m_ImpactWater);
            effect = (ImpactEffect)pool.ActivateAvailableObject();
        }
        else
        {
            //Other effect
            ObjectPool pool = ObjectPoolManager.Instance.GetPool(m_ImpactExplosion);
            effect = (ImpactEffect)pool.ActivateAvailableObject();
        }

        if (effect != null)
            effect.Play(transform.position, Quaternion.identity);

        Deactivate();
    }

    #region PoolableObject
    public override void Initialize()
    {
        m_DefaultMesh = m_MeshFilter.mesh;

        m_DefaultMaterials = new Material[m_MeshRenderer.materials.Length];
        m_MeshRenderer.materials.CopyTo(m_DefaultMaterials, 0);
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
