using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splineFollowerPhysics : MonoBehaviour

{
    public Transform cart;
    public Transform[] railPoints;
    public float speed = 1.0f;
    public float maxSpeedIncrease = 2.0f;
    public float maxSpeedDecrease = 0.5f;
    public float maxTurnSpeedDecrease = 0.5f;
    public float gravity = 10f; 

    private int currentIndex = 0;
    private float distanceToNextPoint;
    private float elapsed = 0f;
    private Rigidbody cartRigidbody;
    private bool isDecline = false;

    void Start()
    {
        cartRigidbody = cart.GetComponent<Rigidbody>();

        
        cart.position = railPoints[0].position;
        cart.rotation = railPoints[0].rotation;
        currentIndex = 0;

        
        distanceToNextPoint = Vector3.Distance(railPoints[currentIndex].position, railPoints[currentIndex + 1].position);
    }

    void Update()
    {
        
        elapsed += Time.deltaTime * GetAdjustedSpeed() / distanceToNextPoint;

        
        if (elapsed >= 1.0f)
        {
            currentIndex++;
            if (currentIndex >= railPoints.Length - 1)
                currentIndex = 0;

            elapsed = 0f;
            distanceToNextPoint = Vector3.Distance(railPoints[currentIndex].position, railPoints[currentIndex + 1].position);

            
            isDecline = railPoints[currentIndex + 1].position.y < railPoints[currentIndex].position.y;
        }

        
        cart.position = Vector3.Lerp(railPoints[currentIndex].position, railPoints[currentIndex + 1].position, elapsed);
        cart.rotation = Quaternion.Lerp(railPoints[currentIndex].rotation, railPoints[currentIndex + 1].rotation, elapsed);

        
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

        
        float adjustedSpeed = speed;

        if (heightDifference > 0) 
            adjustedSpeed *= (1.0f - Mathf.Clamp(heightDifference * maxSpeedDecrease, 0f, maxSpeedDecrease));
        else if (heightDifference < 0)
            adjustedSpeed *= (1.0f + Mathf.Clamp(-heightDifference * maxSpeedIncrease, 0f, maxSpeedIncrease));

        if (turnAngle > 10.0f)
            adjustedSpeed *= (1.0f - Mathf.Clamp(turnAngle * maxTurnSpeedDecrease, 0f, maxTurnSpeedDecrease));

        return adjustedSpeed;
    }
}