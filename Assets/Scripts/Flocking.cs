using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : SteeringBehavior
{
    [SerializeField]
    float radius;
    [SerializeField] Transform target;
    [SerializeField] float arrivemaxRadius;
    new void Start()
    {
        base.Start();

        target = Manager.Instance.ReturnPlayer().transform;

        //velocity = Random.insideUnitCircle * 0.5f;
       // maxSpeed = _maxSpeed = 0.5;
       // maxForce = Random.Range(0.8f, 2);
    }

    void Update()
    {
        OutCamera();
        GoTarget(target.position);
        Align();
        Separate();
        Cohesion();

        ApplySteeringToMotion();
    }
    private void GoTarget(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = target.position - transform.position;
        desiredVelocity.Normalize();

        //calculate the distance between the target and the agent's current location
        float distanceFromTarget = Vector3.Distance(target.position, location);

        //if the agent is close to the target, reduce the desired velocity
        if (distanceFromTarget < arrivemaxRadius) desiredVelocity *= distanceFromTarget;

        //else move towards the target at maximum speed
        else desiredVelocity *= maxSpeed;

        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - velocity, maxForce);
        ApplyForce(steer);
    }
    private void Separate()
    {
        Vector3 separationForce = Vector3.zero;
        int count = 0;

        foreach (Transform a in transform.parent)
        {
            float d = Vector3.Distance(location, a.position);

            if (d > 0 && d < radius)
            {
                Vector3 diff = location - a.position;
                diff.Normalize();
                diff /= d;
                separationForce += diff;
                count++;
            }
        }

        if (count > 0) separationForce /= count;

        if (separationForce.magnitude > 0)
        {
            separationForce.Normalize();
            separationForce *= maxSpeed;
            separationForce = Vector3.ClampMagnitude(separationForce - velocity, maxForce);
            separationForce *= 5;
            ApplyForce(separationForce);
        }
    }

    private void Align()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (Transform a in transform.parent)
        {
            float d = Vector3.Distance(transform.position, a.position);

            if (d > 0 && d < 20)
            {
                sum += a.GetComponent<Flocking>().GetVelocity;
                count++;
            }
        }

        if (count > 0)
        {
            sum /= count;
            sum.Normalize();
            sum *= maxSpeed;
            Vector3 alignmentForce = sum - velocity;
            alignmentForce = Vector3.ClampMagnitude(alignmentForce, maxForce);
            ApplyForce(alignmentForce);
        }
    }

    private void Cohesion()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (Transform a in transform.parent)
        {
            float d = Vector3.Distance(location, a.position);

            if (d > 0 && d < radius)
            {
                sum += a.position;
                count++;
            }
        }

        if (count > 0)
        {
            sum /= count;
            Steer(sum);
        }
    }
    void OutCamera()
    {
        WrapAroundCameraView(gameObject.GetComponent<SpriteRenderer>());
    }
    public Vector3 GetVelocity
    {
        get
        {
            return velocity;
        }
    }
}
