using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Buouncy : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_Offset;
    private Vector3 m_StartPosition;

    private Sequence m_HoverSequence;

    public void StartBuoncy(Vector3 position)
    {
        StopBuoncy();
        m_StartPosition = position;
        transform.DOMove(position, 1.0f).OnComplete(StartLooping);
    }

    public void StopBuoncy()
    {
        m_HoverSequence.Kill();
    }

    private void StartLooping()
    {
        m_HoverSequence = DOTween.Sequence();
        m_HoverSequence.Append(transform.DOMove(m_StartPosition + m_Offset, 0.5f).SetEase(Ease.InOutSine));
        m_HoverSequence.Append(transform.DOMove(m_StartPosition - m_Offset, 1.0f).SetEase(Ease.InOutSine));
        m_HoverSequence.Append(transform.DOMove(m_StartPosition, 0.5f).SetEase(Ease.InOutSine));

        m_HoverSequence.Play().SetLoops(-1);
    }
}