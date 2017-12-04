using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;

    void Update()
    {
        if (m_Camera == null)
            return;

        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}
