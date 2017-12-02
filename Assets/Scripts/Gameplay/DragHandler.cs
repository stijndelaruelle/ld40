using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;
    private ICargo m_CurrentCargo;

    [SerializeField]
    private float m_MinDistance;

    [SerializeField]
    private float m_MaxDistance;
    private float m_ObjectDistance;

    [SerializeField]
    private float m_ScrollIntensity;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleBeginDrag();

        if (Input.GetMouseButton(0))
        {
            HandleDrag();
            HandleScroll();
        }

        if (Input.GetMouseButtonUp(0))
            HandleEndDrag();
    }

    private void HandleBeginDrag()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawLine(ray.origin, ray.origin + (ray.direction * 1000.0f), Color.red, 50.0f);

        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, m_MaxDistance);

        if (hitInfo.collider == null)
            return;

        ICargo cargo = hitInfo.collider.GetComponent<ICargo>();
        if (cargo == null)
            return;

        m_CurrentCargo = cargo;
        m_ObjectDistance = hitInfo.distance;

        m_CurrentCargo.StartDrag();
    }

    private void HandleDrag()
    {
        if (m_CurrentCargo == null)
            return;

        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        Vector3 worldSpace = ray.origin + (ray.direction * m_ObjectDistance);

        m_CurrentCargo.transform.position = worldSpace;
    }

    private void HandleEndDrag()
    {
        if (m_CurrentCargo == null)
            return;

        m_CurrentCargo.StopDrag();
        m_CurrentCargo = null;
    }

    private void HandleScroll()
    {
        if (m_CurrentCargo == null)
            return;

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        m_ObjectDistance += scrollWheel * m_ScrollIntensity;

        if (m_ObjectDistance < m_MinDistance)
            m_ObjectDistance = m_MinDistance;

        if (m_ObjectDistance > m_MaxDistance)
            m_ObjectDistance = m_MaxDistance;
    }
}
