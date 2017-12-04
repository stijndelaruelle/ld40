using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    [SerializeField]
    private bool m_FadeOutOnAwake = false;

    [SerializeField]
    private float m_FadeSpeed = 1.0f;

    [SerializeField]
    private Image m_Image;

    private void Start()
    {
        if (m_FadeOutOnAwake)
        {
            SetAlpha(1.0f);
            FadeOut(null);
        }
    }

    private void SetAlpha(float value)
    {
        Color currentColor = m_Image.color;
        currentColor.a = value;

        m_Image.color = currentColor;
    }

    public void FadeIn(TweenCallback callback)
    {
        m_Image.DOKill();
        m_Image.DOFade(1.0f, m_FadeSpeed).SetEase(Ease.Linear).OnComplete(callback);
    }

    public void FadeOut(TweenCallback callback)
    {
        m_Image.DOKill();
        m_Image.DOFade(0.0f, m_FadeSpeed).SetEase(Ease.Linear).OnComplete(callback);
    }
}
