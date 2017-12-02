using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    // tweaks
    [Header("Camera")]
    [SerializeField]
    private Camera m_ShipCamera;

    [Header("Drag Endpoints")]
    [SerializeField]
    private Vector3 m_LeftPoint;
    [SerializeField]
    private Vector3 m_RightPoint;

    // private core
    private Vector3 m_MouseDelta;
    private bool m_Dragging;
    private Vector3 m_ScreenPos, m_Offset, m_CurPosition;
    private float m_DragDelta;

    private Plane m_DragPlane;

    // accessors
    public float GetDragDelta
    {
        get { return m_DragDelta; }
    }
    public bool IsDragging
    {
        get { return m_Dragging; }
    }

    private void Start()
    {
        transform.position = (m_LeftPoint + m_RightPoint) / 2f;
    }

    private void OnMouseDown()
    {
        if (!m_Dragging)
        {
            RaycastHit _hit;
            Ray _ray = m_ShipCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.collider.CompareTag("Draggable"))
                {
                    m_Dragging = true;
                    m_ScreenPos = m_ShipCamera.WorldToScreenPoint(gameObject.transform.position);

                    m_Offset = gameObject.transform.position - m_ShipCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPos.z));
                }
            }
            else
                m_Dragging = false;
        }
    }

    private void OnMouseDrag()
    {
        if (m_Dragging)
        {
            Vector3 _curScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPos.z);

            Vector3 _curPos = m_ShipCamera.ScreenToWorldPoint(_curScreenPos);
            transform.position = new Vector3(_curPos.x, transform.position.y, transform.position.z);

            if (transform.position.x < m_LeftPoint.x)
                transform.position = transform.TransformPoint(m_LeftPoint);
            if (transform.position.x > m_RightPoint.x)
                transform.position = transform.TransformPoint(m_RightPoint);

        }
    }

    private void OnMouseUp()
    {
        if (m_Dragging)
        {
            m_DragDelta = Remap(Mathf.InverseLerp(m_LeftPoint.x, m_RightPoint.x, transform.position.x), -1f, 1f);
            m_Dragging = false;
        }
    }

    private float Remap(float _val, float _from, float _to)
    {
        return (_val - 0) / (1f - 0f) * (_to - _from) + _from;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_LeftPoint, 0.2f);
        Gizmos.DrawLine(m_LeftPoint, m_RightPoint);
        Gizmos.DrawSphere(m_RightPoint, 0.2f);
    }
}
