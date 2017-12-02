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

    private void Start()
    {
        transform.position = m_LeftPoint;
    }

    // Update is called once per frame
    void Update()
    {
        Ray _ray = m_ShipCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;

        if (!m_Dragging)
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(_ray, out _hit))
                {
                    if (_hit.collider.CompareTag("Draggable"))
                    {
                        m_Dragging = true;
                        m_MouseDelta = Input.mousePosition;
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_Dragging = false;
            }
            else if (Input.GetMouseButton(0))
            {
                UpdateDrag();
            }
        }
    }

    void UpdateDrag()
    {
        if (m_MouseDelta.x < Input.mousePosition.x)
        {
            transform.Translate(1 * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.Translate(-1 * Time.deltaTime, 0, 0);
        }
        float _delta = Mathf.Clamp01(Mathf.InverseLerp(m_LeftPoint.x, m_RightPoint.x, transform.position.x));
        Debug.Log(_delta);
        m_MouseDelta.x = Input.mousePosition.x;

        if (transform.position.x < m_LeftPoint.x)
            transform.position = m_LeftPoint;
        if (transform.position.x > m_RightPoint.x)
            transform.position = m_RightPoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_LeftPoint, m_RightPoint);
    }
}
