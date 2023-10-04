using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splineFollowerPhysics : MonoBehaviour

{
    public Transform cartObject;
    public Transform[] railPoints;
    public float baseSpeed = 1.0f;
    public float maxSpeedIncrease = 2.0f;
    public float maxSpeedDecrease = 0.5f;
    public float maxTurnSpeedDecrease = 0.5f;
    public float gravity = 9.81f; // Gravity acceleration

    private int currentIndex = 0;
    private float distanceToNextPoint;
    private float elapsed = 0f;
    private Rigidbody cartRigidbody;
    private bool isDecline = false;

    void Start()
    {
        if (railPoints.Length < 2)
        {
            Debug.LogError("There must be at least two rail points.");
            enabled = false;
            return;
        }

        cartRigidbody = cartObject.GetComponent<Rigidbody>();

        // Set initial position and rotation
        cartObject.position = railPoints[0].position;
        cartObject.rotation = railPoints[0].rotation;
        currentIndex = 0;

        // Calculate the initial distance to the next point
        distanceToNextPoint = Vector3.Distance(railPoints[currentIndex].position, railPoints[currentIndex + 1].position);
    }

    void Update()
    {
        // Calculate interpolation based on speed
        elapsed += Time.deltaTime * GetAdjustedSpeed() / distanceToNextPoint;

        // Check if we reached the next point
        if (elapsed >= 1.0f)
        {
            currentIndex++;
            if (currentIndex >= railPoints.Length - 1)
                currentIndex = 0;

            elapsed = 0f;
            distanceToNextPoint = Vector3.Distance(railPoints[currentIndex].position, railPoints[currentIndex + 1].position);

            // Check if in decline
            isDecline = railPoints[currentIndex + 1].position.y < railPoints[currentIndex].position.y;
        }

        // Interpolate pos and rot
        cartObject.position = Vector3.Lerp(railPoints[currentIndex].position, railPoints[currentIndex + 1].position, elapsed);
        cartObject.rotation = Quaternion.Lerp(railPoints[currentIndex].rotation, railPoints[currentIndex + 1].rotation, elapsed);

        // Adjust gravity for declines
        if (isDecline)
        {
            cartRigidbody.useGravity = false;
            cartRigidbody.velocity = Vector3.zero;
        }
        else
        {
            cartRigidbody.useGravity = true;
        }
    }

    float GetAdjustedSpeed()
    {
        float heightDifference = railPoints[currentIndex + 1].position.y - railPoints[currentIndex].position.y;
        float turnAngle = Quaternion.Angle(railPoints[currentIndex].rotation, railPoints[currentIndex + 1].rotation);

        // Adjust speed based on height difference, inclines, declines, and turns
        float adjustedSpeed = baseSpeed;

        if (heightDifference > 0) // Incline
            adjustedSpeed *= (1.0f - Mathf.Clamp(heightDifference * maxSpeedDecrease, 0f, maxSpeedDecrease));
        else if (heightDifference < 0) // Decline
            adjustedSpeed *= (1.0f + Mathf.Clamp(-heightDifference * maxSpeedIncrease, 0f, maxSpeedIncrease));

        if (turnAngle > 10.0f) // Turn
            adjustedSpeed *= (1.0f - Mathf.Clamp(turnAngle * maxTurnSpeedDecrease, 0f, maxTurnSpeedDecrease));

        return adjustedSpeed;
    }
}