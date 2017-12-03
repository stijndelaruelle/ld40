using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LootOld : ICargo
{
    [Header("General settings")]
    [SerializeField]
    private int m_State = 1;

    public GameObject StateSmall;
    public GameObject StateMedium;
    public GameObject StateLarge;

    private void Start()
    {
        StartDragEvent += BeginDrag;
    }

    void BeginDrag(ICargo _target)
    {
        m_Gravity = 5f;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.B))
            MakeBigger();
    }

    public void MakeBigger()
    {
        m_State++;
        switch (m_State)
        {
            default:
            case 1:
                StateSmall.SetActive(true);
                break;
            case 2:
                StateSmall.transform.DOScale(1.5f, 2f);
                StateSmall.transform.DOLocalMoveY(1f, 2f);
                break;
            case 3:
                StateSmall.transform.DOScale(2, 2f);
                StateSmall.transform.DOLocalMoveY(1f, 2f);
                break;
            case 4:
                StateSmall.SetActive(false);
                StateMedium.SetActive(true);
                break;
                /*
            case 2:
                StateSmall.SetActive(false);
                StateMedium.SetActive(true);
                break;
            case 3:
                StateMedium.SetActive(false);
                StateLarge.SetActive(true);
                break;
                */
        }
        Debug.Log("Loot state: " + m_State);
        // TODO: change visuals
    }

    private new void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<LootOld>() && m_Gravity == 0)
        {
            Debug.Log(collision.collider.name);
            Destroy(collision.gameObject);
            MakeBigger();
        }

        if (collision.collider.gameObject.CompareTag("Water"))
        {
            if (!gameObject.GetComponent<Buouncy>())
            {
                m_Gravity = 0;
                gameObject.AddComponent<Buouncy>();
            }
        }
    }
}
