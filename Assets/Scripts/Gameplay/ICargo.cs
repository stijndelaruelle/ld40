using DG.Tweening;
using UnityEngine;

public delegate void CargoDelegate(ICargo cargo);
public delegate void CargoEndDelegate(ICargo cargo);


public abstract class ICargo : MonoBehaviour
{
    [Header("Cargo stats")]
    [SerializeField]
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
    private bool m_SwapRotation = false;

    private bool m_IsDragged = false;
    public bool IsDragged
    {
        get { return m_IsDragged; }
    }

    public event CargoDelegate StartDragEvent;
    public event CargoEndDelegate EndDragEvent;

    protected virtual void Update()
    {
        if (m_IsDragged == true)
            return;

        if (m_UseGravity == false)
            return;

        Vector3 gravity = new Vector3(0.0f, -m_Gravity, 0.0f);
        transform.position += gravity * Time.deltaTime;
    }

    public void StartDrag()
    {
        m_IsDragged = true;

        if (StartDragEvent != null)
            StartDragEvent(this);
    }

    public void StopDrag()
    {
        m_IsDragged = false;
        m_UseGravity = true;

        if (EndDragEvent != null)
            EndDragEvent(this);
    }

    //Unity callback
    public void OnCollisionEnter(Collision collision)
    {
        m_UseGravity = false;
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
