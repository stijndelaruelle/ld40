using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreCounter : MonoBehaviour
{
    private Text m_ScoreText;
    private int m_Score;
    private int m_TotalScore;

    private void OnEnable()
    {
        Ship m_Player = FindObjectOfType<Ship>();
        m_ScoreText = GetComponent<Text>();

        foreach (Loot_Gold _cargo in m_Player.GetCargo)
        {
            m_TotalScore += (int)_cargo.GetValue;
        }
        m_TotalScore = 1000;
        StartCoroutine(DrawScore());

    }

    IEnumerator DrawScore()
    {
        while (m_Score <= m_TotalScore)
        {
            m_ScoreText.text = m_Score.ToString();
            m_Score++;
            if (m_Score % 100 == 0)
            {
                StartCoroutine(QuickScale());
            }
            yield return new WaitForEndOfFrame();
        }
        m_Score = m_TotalScore;
    }

    IEnumerator QuickScale()
    {
        float _time = 0;
        while (_time <= 1f)
        {
            if (_time < 0.5f)
            {
                m_ScoreText.rectTransform.localScale.Set(2f * Time.deltaTime, 2f * Time.deltaTime, 2f * Time.deltaTime);
            }
            else
            {
                m_ScoreText.rectTransform.localScale.Set(1f * Time.deltaTime, 1f * Time.deltaTime, 1f * Time.deltaTime);
            }
            _time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        m_ScoreText.rectTransform.sizeDelta = new Vector2(1, 1);
    }
}
