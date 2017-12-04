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

    private GameObject m_RumObject;

    private void Start()
    {
        if (transform.childCount <= 0)
        {
            Debug.LogWarning("RumPanel needs at least 1 child to be used as 'prefab'");
        }

        m_RumObject = transform.GetChild(0).gameObject;
    }

    public void StartBuying(float money)
    {
        StartCoroutine(BuyRumRoutine(money));
    }

    private IEnumerator BuyRumRoutine(float currentValue)
    {
        while (currentValue >= m_RumPrice)
        {
            yield return new WaitForSeconds(m_BuyRumSpeed);

            //Create more rum
            //...


            //Pay for it
            currentValue -= m_RumPrice;
            m_Text.text = ((int)currentValue).ToString();
        }
    }
}
