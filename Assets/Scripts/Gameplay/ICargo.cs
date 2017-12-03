﻿using DG.Tweening;
using UnityEngine;

public delegate void CargoDelegate(ICargo cargo);


public abstract class ICargo : MonoBehaviour
{
    //[SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_Position; //-1 to 1
    public float Position
    {
        get { return m_Position; }
        set { m_Position = value; }
    }

    [Header("Cargo stats")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float m_Weight;
    public float Weight
    {
        get { return m_Weight; }
        set { m_Weight = value; }
    }

    [SerializeField]
    protected float m_Gravity;

    [SerializeField]
    private BoxCollider m_Collider;

    [Header("Effects")]
    [SerializeField]
    private GameObject m_Projection;

    [SerializeField]
    private Renderer m_Renderer;
    public Renderer Renderer
    {
        get { return m_Renderer; }
    }

    [SerializeField]
    private MeshFilter m_MeshFilter;
    public MeshFilter MeshFilter
    {
        get { return m_MeshFilter; }
    }

    [SerializeField]
    private Material m_ActiveMaterial;
    private Material[] m_DefaultMaterials;

    [SerializeField]
    private Vector3 m_RotationSpeed;

    private bool m_IsDragged = false;
    public bool IsDragged
    {
        get { return m_IsDragged; }
    }

    private bool m_IsGrounded = false;

    public bool CanUse
    {
        get
        {
            return (m_IsDragged == false && m_IsGrounded == true);
        }
    }

    public event CargoDelegate StartDragEvent;
    public event CargoDelegate EndDragEvent;
    public event CargoDelegate DestroyEvent;

    private void Start()
    {
        Material[] matArray = m_Renderer.materials;

        m_DefaultMaterials = new Material[matArray.Length];
        for (int i = 0; i < matArray.Length; ++i)
        {
            m_DefaultMaterials[i] = matArray[i];
        }
    }

    protected virtual void Update()
    {
        if (m_IsDragged == true)
            return;

        //Is Grounded Check
        Vector3 bottomPosition = transform.position + m_Collider.center + (Vector3.down * ((transform.localScale.y * m_Collider.size.y) * 0.5f));

        RaycastHit hitInfo;
        Physics.Raycast(bottomPosition, Vector3.down, out hitInfo, m_Gravity * Time.deltaTime);

        Debug.DrawLine(bottomPosition, bottomPosition + (Vector3.down * 100.0f), Color.red);

        m_IsGrounded = (hitInfo.collider != null && hitInfo.collider != m_Collider);

        if (!m_IsGrounded)
        {
            Vector3 gravity = new Vector3(0.0f, -m_Gravity, 0.0f);
            transform.position += gravity * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        if (DestroyEvent != null)
            DestroyEvent(this);
    }

    public void StartDrag()
    {
        m_IsDragged = true;
        m_Collider.enabled = false;

        //Detach
        transform.parent = null;

        if (StartDragEvent != null)
            StartDragEvent(this);
    }

    public void HandleDrag(Vector3 newWorldSpace, Ray ray)
    {
        transform.position = newWorldSpace;

        //Slowly rotate
        transform.Rotate(new Vector3(m_RotationSpeed.x * Time.deltaTime,
                                     m_RotationSpeed.y * Time.deltaTime,
                                     m_RotationSpeed.z * Time.deltaTime));


        //Check if we are over an object that uses cargo (so far only the canon)
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 1000.0f);// LayerMask.NameToLayer("Ignore Raycast"));

        if (hitInfo.collider != null)
        {
            Canon canon = hitInfo.collider.GetComponent<Canon>();

            if (canon != null)
            {
                m_Projection.SetActive(false);

                if (m_Renderer != null)
                {
                    Material[] matArray = m_Renderer.materials;
                    for (int i = 0; i < matArray.Length; ++i)
                    {
                        matArray[i] = m_ActiveMaterial;
                    }

                    m_Renderer.materials = matArray;
                }
                return;
            }
        }

        //We didn't hit a canon
        m_Projection.SetActive(true);

        if (m_Renderer != null)
        {
            Material[] matArray = m_Renderer.materials;
            matArray = m_DefaultMaterials;
            m_Renderer.materials = matArray;
        }
    }

    public void StopDrag()
    {
        m_IsDragged = false;
        m_Collider.enabled = true;

        //Check if we are hovering over the ship, if so parent it.
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1000.0f);// LayerMask.NameToLayer("Ignore Raycast"));

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                transform.parent = hitInfo.collider.transform;
            }
        }

        if (EndDragEvent != null)
            EndDragEvent(this);
    }

    //Unity callback
    public void OnCollisionEnter(Collision collision)
    {
        //Doesn't work 100% but good enough for us        //RaycastHit hitInfo;
        //Physics.Raycast

        if (collision.collider.CompareTag("Player"))
        {
            transform.parent = collision.collider.transform;
        }


        //Average the collision points
        Vector3 avgContact = Vector3.zero;
        for (int i = 0; i < collision.contacts.Length; ++i)
        {
            avgContact += collision.contacts[i].point;
        }

        avgContact /= collision.contacts.Length;

        //Cast from there a ray down to find the distance we are in the ground
        RaycastHit hitInfo;
        Physics.Raycast(avgContact, Vector3.down, out hitInfo, 10.0f * Time.deltaTime);

        if (hitInfo.collider == m_Collider)
        {
            transform.position += hitInfo.normal * hitInfo.distance;
        }
    }

    private void OnValidate()
    {
    }

    //Utility
    private float RemapPosition(float position)
    {
        //https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        //return (m_Position - (-1)) / (1 - (-1)) * (1 - 0) + 0;

        return ((position + 1) / 2);
    }
}
