using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : MonoBehaviour
{

    [SerializeField]
    private Camera m_ShipCamera;

    [SerializeField]
    private Vector3 m_LeftPoint, m_RightPoint;
    private Vector3 m_MouseDelta;
    private bool m_Dragging;

    private Vector3 m_ScreenPos, m_Offset, m_CurPosition;

    private void Start()
    {
        transform.position = (m_LeftPoint + m_RightPoint) / 2f;
    }

    void OnMouseDown()
    {
        m_ScreenPos = m_ShipCamera.WorldToScreenPoint(gameObject.transform.position);

        m_Offset = gameObject.transform.position - m_ShipCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPos.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPos.z);

        Vector3 curPosition = m_ShipCamera.ScreenToWorldPoint(curScreenPoint);// - m_Offset;
        transform.position = new Vector3(curPosition.x, transform.position.y, transform.position.z);

        if (transform.position.x < m_LeftPoint.x)
            transform.position = m_LeftPoint;
        if (transform.position.x > m_RightPoint.x)
            transform.position = m_RightPoint;
    }

    private void OnMouseUp()
    {
        float _delta = Remap(Mathf.InverseLerp(m_LeftPoint.x, m_RightPoint.x, transform.position.x), -1f, 1f);
        Debug.Log(_delta);
    }

    float Remap(float _val, float _from, float _to)
    {
        return (_val - 0) / (_to - 1f) * (_to - _from) + _from;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_LeftPoint, 0.5f);
        Gizmos.DrawLine(m_LeftPoint, m_RightPoint);
        Gizmos.DrawSphere(m_RightPoint, 0.5f);
    }
}
