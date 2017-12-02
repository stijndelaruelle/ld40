using DG.Tweening;
using UnityEngine;

public abstract class ICargoOld : MonoBehaviour
{
    [Header("Cargo stats")]
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float m_Position; //-1 to 1
    private float m_CurrentPosition;
    public float Position
    {
        get { return m_CurrentPosition; }
        set { SetPosition(value); }
    }

    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float m_Weight;
    private float m_CurrentWeight;
    public float Weight
    {
        get { return m_Weight; }
        set { SetWeight(value); }
    }

    [Header("Cargo animation")]
    [SerializeField]
    private Transform m_LeftPosition;

    [SerializeField]
    private Transform m_RightPosition;

    [SerializeField]
    private float m_SnapSpeed; //Speed to change from -1 to 1
    //private Sequence m_MoveSequence;

    [SerializeField]
    private bool m_SnapPosition;

    private bool m_CanUse;
    public bool CanUse
    {
        get { return m_CanUse; }
    }

    protected virtual void Update()
    {
        HandlePositionSnapping();
    }

    private void HandlePositionSnapping()
    {
        if (m_CurrentPosition != m_Position)
        {
            m_CanUse = false;
            float dir = -1;
            if (m_Position > m_CurrentPosition) { dir = 1; }

            m_CurrentPosition += dir * m_SnapSpeed * Time.deltaTime;

            float newDir = -1;
            if (m_Position > m_CurrentPosition) { newDir = 1; }

            //We flipped
            if (dir + newDir == 0)
            {
                m_CurrentPosition = m_Position;
                m_CanUse = true;
            }

            Vector3 newWorldPosition = Vector3.Lerp(m_LeftPosition.position, m_RightPosition.position, RemapPosition(m_CurrentPosition));
            Quaternion newWorldRotation = Quaternion.Lerp(m_LeftPosition.rotation, m_RightPosition.rotation, RemapPosition(m_CurrentPosition));

            transform.position = newWorldPosition;
            transform.rotation = newWorldRotation;
        }
    }


    //Mutators
    private void SetPosition(float position)
    {
        if (m_SnapPosition)
        {
            position = Mathf.Sign(position);
        }

        m_Position = position;
    }

    private void SetWeight(float weight)
    {
        m_Weight = weight;
    }


    //Unity callback
    private void OnValidate()
    {
        if (m_CanUse == false)
            return;

        SetPosition(m_Position);
        SetWeight(m_Weight);
    }

    //Utility
    private float RemapPosition(float position)
    {
        //https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        //return (m_Position - (-1)) / (1 - (-1)) * (1 - 0) + 0;

        return ((position + 1) / 2);
    }
}
