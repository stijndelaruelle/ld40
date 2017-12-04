using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;
    private ICargo m_CurrentDraggingCargo;
    private ICargo m_CurrentHoverCargo;

    public bool IsDragging
    {
        get { return (m_CurrentDraggingCargo != null); }
    }

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
        {
            HandleBeginDrag();
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleEndDrag();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            HandleDrag();
            HandleScroll();
            return;
        }

        HandleHover();
    }

    private void HandleBeginDrag()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawLine(ray.origin, ray.origin + (ray.direction * 1000.0f), Color.red, 50.0f);

        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, m_MaxDistance);// LayerMask.NameToLayer("Ignore Raycast"));

        if (hitInfo.collider == null)
            return;

        ICargo cargo = hitInfo.collider.GetComponent<ICargo>();
        if (cargo == null)
            return;

        m_CurrentDraggingCargo = cargo;
        m_ObjectDistance = hitInfo.distance;

        m_CurrentDraggingCargo.StartDrag();
    }

    private void HandleDrag()
    {
        if (m_CurrentDraggingCargo == null)
            return;

        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        Vector3 worldSpace = ray.origin + (ray.direction * m_ObjectDistance);

        m_CurrentDraggingCargo.HandleDrag(worldSpace, ray);
    }

    private void HandleEndDrag()
    {
        if (m_CurrentDraggingCargo == null)
            return;

        //Check if we are over an object that uses cargo (so far only the canon)
        RaycastHit hitInfo;
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hitInfo, 1000.0f);// LayerMask.NameToLayer("Ignore Raycast"));

        if (hitInfo.collider != null)
        {
            Canon canon = hitInfo.collider.GetComponent<Canon>();

            if (canon != null)
            {
                canon.Fire(m_CurrentDraggingCargo.MeshFilter.mesh, m_CurrentDraggingCargo.Renderer.materials);
                GameObject.Destroy(m_CurrentDraggingCargo.gameObject); //POOL!
                return;
            }
        }

        m_CurrentDraggingCargo.StopDrag();
        m_CurrentDraggingCargo = null;
    }

    private void HandleScroll()
    {
        if (m_CurrentDraggingCargo == null)
            return;

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        m_ObjectDistance += scrollWheel * m_ScrollIntensity;

        if (m_ObjectDistance < m_MinDistance)
            m_ObjectDistance = m_MinDistance;

        if (m_ObjectDistance > m_MaxDistance)
            m_ObjectDistance = m_MaxDistance;
    }

    private void HandleHover()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, m_MaxDistance);

        if (hitInfo.collider == null)
            return;

        ICargo cargo = hitInfo.collider.GetComponent<ICargo>();

        if (m_CurrentHoverCargo != null && m_CurrentHoverCargo != cargo)
        {
            m_CurrentHoverCargo.StopHover();
        }

        m_CurrentHoverCargo = cargo;

        if (cargo != null)
            m_CurrentHoverCargo.StartHover();
    }
}
