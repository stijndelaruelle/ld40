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
            m_TotalScore += (int)_cargo.Value;
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
            yield return new WaitForEndOfFrame();
        }
        m_Score = m_TotalScore;
    }
}
