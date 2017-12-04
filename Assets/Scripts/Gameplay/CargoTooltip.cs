using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoTooltip : MonoBehaviour
{
    [SerializeField]
    private ICargo m_Cargo;

    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    private void Start()
    {
        m_Cargo.StartDragEvent += OnStartDrag;
    }

    private void OnStartDrag(ICargo cargo)
    {
        m_SpriteRenderer.DOFade(0.0f, 1.0f).OnComplete(OnFadeComplete);
    }

    private void OnFadeComplete()
    {
        gameObject.SetActive(false);
    }
}
