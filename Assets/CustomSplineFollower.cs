using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CustomSplineFollower : MonoBehaviour
{

    public SplineContainer spline;
    public Transform cart;
    public Transform[] points;
    public Transform p1;
    public Transform p2;
    public float speed = 1f;
    float distancePercentage = 0f;

    float splineLength;


    public List<GameObject> vertices;
    public List<string> trackType;


    public float baseSpeed = 1.0f;
    public float maxSpeedIncrease = 2.0f;
    public float maxSpeedDecrease = 0.5f;




    // Start is called before the first frame update
    void Start()
    {
        splineLength = spline.CalculateLength();
    }

    // Update is called once per frame
    void Update()
    {

        spline.
        distancePercentage += speed * Time.deltaTime / splineLength;


        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        

        if(distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;

        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(distancePercentage));

        //var remappedForward = new Vector3(0, 1, 0);
        //var remappedUp = new Vector3(0, 0, 1);
        //var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward, remappedUp));


        transform.rotation = Quaternion.LookRotation(forward, transform.up);

        if (isBetweenTwoPoints(cart.position,p1.position , p2.position))
        {

            Debug.Log(transform.position);
        }

    }

    bool isBetweenTwoPoints(Vector3 x, Vector3 a, Vector3 b)
    {
        float distanceAX = Vector3.Distance(a, x);
        float distanceBX = Vector3.Distance(b, x);
        float distanceAB = Vector3.Distance(a, b);

        Debug.Log(Mathf.Approximately(distanceAX + distanceBX, distanceAB));

        //if ((distanceAX + distanceBX) == distanceAB) {
        //    Debug.Log(distanceAB);
            
        //}

        return Mathf.Approximately(distanceAX + distanceBX, distanceAB);
    }

    float GetAdjustedSpeed(currentPosition, nextPosition)
    {
        float heightDifference = nextPosition.y - currentPosition.y;
        //float turnAngle = Quaternion.Angle(railPoints[currentIndex].rotation, railPoints[currentIndex + 1].rotation);

        // Adjust speed based on height difference, inclines, declines, and turns
        float adjustedSpeed = baseSpeed;

        if (heightDifference > 0) // Incline
            adjustedSpeed *= (1.0f - Mathf.Clamp(heightDifference * maxSpeedDecrease, 0f, maxSpeedDecrease));
        else if (heightDifference < 0) // Decline
            adjustedSpeed *= (1.0f + Mathf.Clamp(-heightDifference * maxSpeedIncrease, 0f, maxSpeedIncrease));

        //if (turnAngle > 10.0f) // Turn
        //    adjustedSpeed *= (1.0f - Mathf.Clamp(turnAngle * maxTurnSpeedDecrease, 0f, maxTurnSpeedDecrease));

        return adjustedSpeed;
    }

    private void FixedUpdate()
    {
        // Calculate the position and orientation of the cart on the spline
        Vector3 position = spline.GetPoint(distance);
        Quaternion rotation = spline.GetRotation(distance);

        // Calculate the velocity along the spline to apply forces
        Vector3 velocity = (position - lastPosition) / Time.fixedDeltaTime;

        // Apply gravity to the cart
        Vector3 gravityForce = Vector3.down * gravity * rb.mass;
        rb.AddForce(gravityForce);

        // Apply the calculated velocity as a force
        rb.velocity = velocity;

        // Set the position and rotation of the cart
        rb.MovePosition(position);
        rb.MoveRotation(rotation);

        // Update the distance traveled along the spline
        distance += speed * Time.fixedDeltaTime;

        // Check if the roller coaster has reached the end of the track
        if (distance >= spline.Length)
        {
            // You can add logic to reset or end the ride here
        }

        // Update the last position for velocity calculation
        lastPosition = position;
    }


}
