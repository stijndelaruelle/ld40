using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DubloonCounter : MonoBehaviour
{
    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private Ship m_Ship;

    [SerializeField]
    private float m_StartDelay;

    [SerializeField]
    private int m_CountSpeed;

    [SerializeField]
    private RumPanel m_RumPanel;

    private void Start()
    {
        m_Text.text = "0";
        StartCoroutine(CountUpRoutine());
    }

    private IEnumerator CountUpRoutine()
    {
        yield return new WaitForSeconds(m_StartDelay);

        float currentValue = 0.0f;
        float targetValue = 1000.0f;//m_Ship.GetLootValue();

        while (targetValue > currentValue)
        {
            currentValue += m_CountSpeed * Time.deltaTime;

            if (currentValue > targetValue)
                currentValue = targetValue;

            m_Text.text = ((int)currentValue).ToString();

            yield return new WaitForEndOfFrame();
        }

        m_RumPanel.StartBuying(currentValue);
    }
}
