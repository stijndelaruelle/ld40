using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AnimationStep
{
    [SerializeField]
    private Vector2 m_Position;
    public Vector2 Position
    {
        get { return m_Position; }
    }

    [SerializeField]
    private float m_PreDelay;
    public float PreDelay
    {
        get { return m_PreDelay; }
    }

    [SerializeField]
    private float m_PostDelay;
    public float PostDelay
    {
        get { return m_PostDelay; }
    }

    [SerializeField]
    private float m_Duration;
    public float Duration
    {
        get { return m_Duration; }
    }

    [SerializeField]
    private Ease m_EaseMethod;
    public Ease EaseMethod
    {
        get { return m_EaseMethod; }
    }
}

public class AnimationStepChain : MonoBehaviour
{
    [SerializeField]
    private bool m_PlayOnAwake = true;

    [SerializeField]
    private List<AnimationStep> m_AnimationSteps;

    private RectTransform m_RectTransform;
    private Sequence m_Sequence;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (m_PlayOnAwake)
            Play();
    }

    public void Play()
    {
        if (m_AnimationSteps.Count <= 0)
            return;

        if (m_Sequence != null)
            m_Sequence.Kill();

        gameObject.SetActive(true);

        m_Sequence = DOTween.Sequence();

        foreach (AnimationStep animStep in m_AnimationSteps)
        {
            if (animStep.PreDelay > 0) { m_Sequence.AppendInterval(animStep.PreDelay); }
            m_Sequence.Append(m_RectTransform.DOAnchorPos(animStep.Position, animStep.Duration).SetEase(animStep.EaseMethod));
            if (animStep.PreDelay > 0) { m_Sequence.AppendInterval(animStep.PostDelay); }
        }

        m_Sequence.Play();

        //Avoid first frame snapping
        if (m_AnimationSteps[0].PreDelay <= 0)
        {
            m_RectTransform.anchoredPosition = m_AnimationSteps[0].Position;
        }
    }
}
