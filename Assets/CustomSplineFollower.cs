using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CustomSplineFollower : MonoBehaviour
{

    public SplineContainer spline;
    public Transform cart;
    public Transform p1;
    public Transform p2;
    public float speed = 1f;
    float distancePercentage = 0f;

    float splineLength;

    // Start is called before the first frame update
    void Start()
    {
        splineLength = spline.CalculateLength();
    }

    // Update is called once per frame
    void Update()
    {

        
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

        if ((distanceAX + distanceBX) == distanceAB) {
        }

        return Mathf.Approximately(distanceAX + distanceBX, distanceAB);
    }
}
