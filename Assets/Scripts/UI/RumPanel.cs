using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RumPanel : MonoBehaviour
{
    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private int m_RumPrice;

    [SerializeField]
    private float m_BuyRumSpeed;

    [SerializeField]
    private AnimationStepChain m_RumBottlePrefab;

    [SerializeField]
    private Vector3 m_PerBottleOffset;

    public void StartBuying(float money)
    {
        StartCoroutine(BuyRumRoutine(money));
    }

    private IEnumerator BuyRumRoutine(float currentValue)
    {
        float amountOfBottles = (int)(currentValue / m_RumPrice);
        Debug.Log(amountOfBottles);
        Vector3 startPosition = Vector3.zero;
        startPosition -= (amountOfBottles * (m_PerBottleOffset * 0.5f)) - (m_PerBottleOffset * 0.5f);

        Vector3 nextBottlePosition = startPosition;

        while (currentValue >= m_RumPrice)
        {
            //Create more rum
            AnimationStepChain bottle = Instantiate(m_RumBottlePrefab, transform.position, Quaternion.identity, transform);
            bottle.GetAnimationStep(1).Position = nextBottlePosition;
            bottle.Play();

            nextBottlePosition += m_PerBottleOffset;

            //Pay for it
            currentValue -= m_RumPrice;
            m_Text.text = ((int)currentValue).ToString("00000");

            yield return new WaitForSeconds(m_BuyRumSpeed);
        }
    }
}
