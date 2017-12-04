using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private Image m_Line;

    [SerializeField]
    private Vector3 m_HoverOffset;

    [SerializeField]
    private float m_HoverSpeed;

    private Vector3 m_StartPosition;

    private bool m_IsHovering;
    private float m_Timer;

    private void Awake()
    {
        m_StartPosition = transform.position;
    }

    private void Update()
    {
        m_Timer += m_HoverSpeed * Time.deltaTime;

        if (m_Timer > Mathf.PI * 2)
            m_Timer -= (Mathf.PI * 2);

        if (m_IsHovering)
        {       
            transform.position += new Vector3(0.0f, Mathf.Sin(m_Timer), 0.0f);
        }
    }

    public void StartHovering()
    {
        m_IsHovering = true;
    }

    private void StopHovering()
    {
        m_IsHovering = false;
        transform.DOMove(m_StartPosition, 0.25f).SetEase(Ease.InOutSine);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Line.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Line.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartHovering();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StopHovering();
    }
}
