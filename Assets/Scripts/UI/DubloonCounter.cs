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
    private float m_EndDelay;

    [SerializeField]
    private int m_CountTime;

    [SerializeField]
    private RumPanel m_RumPanel;

    private void Start()
    {
        m_Text.text = "00000";
        StartCoroutine(CountUpRoutine());
    }

    private IEnumerator CountUpRoutine()
    {
        yield return new WaitForSeconds(m_StartDelay);

        float currentValue = 0.0f;
        float targetValue = m_Ship.GetLootValue();
        float timer = 0.0f;

        while (targetValue > currentValue)
        {
            //Variable time
            //currentValue += m_CountSpeed * Time.deltaTime;

            //Set time
            timer += Time.deltaTime;
            float fraction = (timer / m_CountTime);

            currentValue = Mathf.Lerp(0, targetValue, fraction);

            if (currentValue > targetValue)
                currentValue = targetValue;

            m_Text.text = ((int)currentValue).ToString("00000");

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(m_EndDelay);
        m_RumPanel.StartBuying(currentValue);
    }
}
