using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookCameraOverride : MonoBehaviour
{
    private string m_InputAxisNameX;
    private string m_InputAxisNameY;

    [Header ("Required References")]
    [SerializeField]
    private CinemachineFreeLook m_FreeLookCamera;

    [SerializeField]
    private DragHandler m_DragHandler;

    private void Start()
    {
        m_InputAxisNameX = m_FreeLookCamera.m_XAxis.m_InputAxisName;
        m_InputAxisNameY = m_FreeLookCamera.m_YAxis.m_InputAxisName;
}

    private void Update()
    {
        if (Input.GetMouseButton(1) && m_DragHandler.IsDragging == false)
        {
            m_FreeLookCamera.m_XAxis.m_InputAxisName = m_InputAxisNameX;
            m_FreeLookCamera.m_YAxis.m_InputAxisName = m_InputAxisNameY;
        }
        else
        {
            m_FreeLookCamera.m_XAxis.m_InputAxisName = "";
            m_FreeLookCamera.m_YAxis.m_InputAxisName = "";

            m_FreeLookCamera.m_XAxis.m_InputAxisValue = 0.0f;
            m_FreeLookCamera.m_YAxis.m_InputAxisValue = 0.0f;
        }
    }
}
