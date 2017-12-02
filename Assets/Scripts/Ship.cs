using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField]
    private float m_MaxSpeed;

    [SerializeField]
    private float m_WeightToSpeedRatio; //F.e.: if this is 5, it means: 5kg = 1 less speed

    [Header("Turning")]
    [SerializeField]
    private float m_MaxTiltAngle; //Higher than this and the ship sinks

    [SerializeField]
    private float m_WeightToAngleRatio; //F.e.: if this is 5, it means:  5kg = add 1 degree per second

    [Header("Weight")]
    [SerializeField]
    private float m_MaxWeight; //Higher than this and the ship sinks

    [SerializeField]
    private List<ICargo> m_Cargo;

    private Vector2 m_CurrentDirection;

    private void Update()
    {
        //Determine the speed & direction of the ship
        float cumulativeWeight = 0;
        float relativeWeight = 0;

        foreach(ICargo cargo in m_Cargo)
        {
            cumulativeWeight += cargo.Weight;
            relativeWeight += cargo.Position * cargo.Weight;
        }

        //Calculate speed
        float lostSpeed = cumulativeWeight / m_WeightToSpeedRatio;
        float currentSpeed = m_MaxSpeed - lostSpeed;

        //Calculate angle
        float addedAngle = relativeWeight / m_WeightToAngleRatio;

        Vector2 addedDirection = Vector2.zero;
        addedDirection = addedDirection.DegreeToVector2(addedAngle);
        addedDirection.Normalize();

        float currentAngle = Mathf.Atan2(addedDirection.y, addedDirection.x) * Mathf.Rad2Deg;

        m_CurrentDirection.x += addedDirection.y * Time.deltaTime;
        m_CurrentDirection.y += addedDirection.x * Time.deltaTime;

        m_CurrentDirection.Normalize();

        //Actually move
        Vector2 addedPosition = (currentSpeed * m_CurrentDirection) * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + addedPosition.x,
                                         transform.position.y,
                                         transform.position.z + addedPosition.y);

        //Visually rotate
        //float tiltAngle = m_MaxTiltAngle
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, currentAngle, -addedAngle));
    }

    private void OnDrawGizmos()
    {
        Vector3 lineEnd = transform.position;
        lineEnd.x += m_CurrentDirection.x * 2.0f;
        lineEnd.z += m_CurrentDirection.y * 2.0f;

        Gizmos.DrawLine(transform.position, lineEnd);
    }
}
