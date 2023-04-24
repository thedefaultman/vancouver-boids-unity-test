using System.Collections;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField]
    Transform wingsMesh;

    [SerializeField]
    Rigidbody thisRigidbody;

    [SerializeField]
    float flapWaitTime = 0.4f;

    [SerializeField]
    float maxFlapWaitOffset = 0.1f;

    [SerializeField]
    float flapDuration = 0.05f;


    [SerializeField]
    float flySpeed = 5f;


    [SerializeField]
    float obstacleAvoidanceConeAngle = 30f;

    [SerializeField]
    float obstacleAvoidanceDistance = 1f;

    [SerializeField]
    int obstacleAvoidanceSamples = 10;

    [SerializeField]
    float avoidanceStrength = 0.2f;

    [SerializeField]
    float diveStrength = 0.2f;


    [SerializeField]
    int avoidanceCheckRate = 10;

    [SerializeField]
    float birdAvoidanceStrength = 2f;

    [SerializeField]
    float birdAvoidanceDistance = 1f;

    [SerializeField]
    float birdAlignmentStrength = 5f;

    int avoidanceCheckCounter;
    void Start()
    {
        avoidanceCheckCounter = Random.Range(0, avoidanceCheckRate);

        StartCoroutine(FlapWings());
        thisRigidbody.velocity = this.transform.forward * flySpeed;
        thisRigidbody.AddForce(GetObstacleAvoidanceForce());

        birdAlignmentStrength = Random.Range(-0.1f, 1) * birdAlignmentStrength;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(avoidanceCheckCounter==avoidanceCheckRate){
            thisRigidbody.AddForce(GetObstacleAvoidanceForce());
            avoidanceCheckCounter = 0;
        }


        thisRigidbody.rotation = Quaternion.LookRotation(thisRigidbody.velocity, Vector3.up);
        thisRigidbody.AddForce(this.transform.forward * Mathf.Max(flySpeed - thisRigidbody.velocity.magnitude));
        avoidanceCheckCounter++;
    }


    Vector3 GetObstacleAvoidanceForce()
    {

        var avoidanceForce = new Vector3();
        int hitCount = 0;
        for (int i = 0; i < obstacleAvoidanceSamples; i++){
            float currentAngle = i / (float)obstacleAvoidanceSamples * 360f;
            var currentDirection = Quaternion.AngleAxis(obstacleAvoidanceConeAngle, this.transform.up) * this.transform.forward;
            currentDirection = Quaternion.AngleAxis(currentAngle, this.transform.forward) * currentDirection;
            if(Physics.Raycast(this.transform.position, currentDirection, out var hit, obstacleAvoidanceDistance)){
                avoidanceForce += -currentDirection * avoidanceStrength * (1f - hit.distance / obstacleAvoidanceDistance);
                hitCount++;
            }
        }


        avoidanceForce += Vector3.down * diveStrength * (1f - hitCount / (float)obstacleAvoidanceSamples);

        Collider[] nearbyBirds = Physics.OverlapSphere(this.transform.position, birdAvoidanceDistance, LayerMask.GetMask("Bird"));

        var averageBirdVelocity = Vector3.zero;
        foreach (var bird in nearbyBirds){



            var birdDirection = bird.transform.position - this.transform.position;
            float birdDistance = birdDirection.magnitude;
            birdDirection.Normalize();
            avoidanceForce += -birdDirection * birdAvoidanceStrength * (1f - birdDistance / birdAvoidanceDistance);
            averageBirdVelocity += bird.GetComponent<Rigidbody>().velocity;
        }


        averageBirdVelocity /= nearbyBirds.Length;

        thisRigidbody.velocity = Vector3.Lerp(thisRigidbody.velocity, averageBirdVelocity, birdAlignmentStrength);

        return avoidanceForce;
    }



    IEnumerator FlapWings()
    {
        while (true){

            float waitTime = flapWaitTime + Random.Range(-maxFlapWaitOffset, 0);
            yield return new WaitForSeconds(waitTime);
            wingsMesh.localScale = Vector3.Scale(wingsMesh.localScale, new Vector3(1, 1, -1));
            yield return new WaitForSeconds(flapDuration);
            wingsMesh.localScale = Vector3.Scale(wingsMesh.localScale, new Vector3(1, 1, -1));
        }
    }
}