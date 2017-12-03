using DG.Tweening;
using UnityEngine;

public delegate void CargoDelegate(ICargo cargo);


public abstract class ICargo : MonoBehaviour
{
    [Header("Cargo stats")]
    //[SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_Position; //-1 to 1
    public float Position
    {
        get { return m_Position; }
        set { m_Position = value; }
    }

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
    private bool m_UseGravity = true;

    [SerializeField]
    private Collider m_Collider;

    [Header("Effects")]
    [SerializeField]
    private GameObject m_Projection;

    [SerializeField]
    private Renderer m_Renderer;

    [SerializeField]
    private Material m_DefaultMaterial;

    [SerializeField]
    private Material m_ActiveMaterial;

    private bool m_IsDragged = false;
    public bool IsDragged
    {
        get { return m_IsDragged; }
    }

    public bool CanUse
    {
        get
        {
            return (m_IsDragged == false && m_UseGravity == false);
        }
    }

    public event CargoDelegate StartDragEvent;
    public event CargoDelegate EndDragEvent;
    public event CargoDelegate DestroyEvent;

    protected virtual void Update()
    {
        if (m_IsDragged == true)
            return;

        if (m_UseGravity == false)
            return;

        Vector3 gravity = new Vector3(0.0f, -m_Gravity, 0.0f);
        transform.position += gravity * Time.deltaTime;
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

        if (StartDragEvent != null)
            StartDragEvent(this);
    }

    public void HandleDrag(Vector3 newWorldSpace, Ray ray)
    {
        transform.position = newWorldSpace;

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
                    m_Renderer.material = m_ActiveMaterial;
                return;
            }
        }

        //We didn't hit a canon
        m_Projection.SetActive(true);

        if (m_Renderer != null)
            m_Renderer.material = m_DefaultMaterial;
    }

    public void StopDrag()
    {
        m_IsDragged = false;
        m_UseGravity = true;

        m_Collider.enabled = true;

        if (EndDragEvent != null)
            EndDragEvent(this);
    }

    //Unity callback
    public void OnCollisionEnter(Collision collision)
    {
        m_UseGravity = false;
        if (!collision.collider.CompareTag("Water"))
            transform.parent = collision.collider.transform;
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
