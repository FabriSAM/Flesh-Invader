using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAction : StateAction
{
    private GameObject patroller;
    //private NavMeshAgent navMeshAgent;

    private Vector3[] patrolPositions;
    private int positionIndex;
    private float acceptableRadius;

    private Vector3 currentTransformToReach;
    private Rigidbody rigidbody;
    private float patrolSpeed;

    public PatrolAction(GameObject patroller, Vector3[] patrolPositions, float acceptableRadius, float patrolSpeed)
    {
        this.patroller = patroller;
        //navMeshAgent = patroller.GetComponent<navMeshAgent>();
        rigidbody = patroller.GetComponent<Rigidbody>();

        this.patrolPositions = patrolPositions;
        this.acceptableRadius = acceptableRadius;
        this.patrolSpeed = patrolSpeed;
    }

    public override void OnEnter()
    {
        if (patrolPositions.Length != 0)
            currentTransformToReach = patrolPositions[0];
        else 
            currentTransformToReach = patroller.transform.position;
        InternalSetVelocity();
    }

    public override void OnUpdate()
    {
        InternalSetVelocity();
        Vector3 positionToReachLocal = patroller.transform.InverseTransformPoint(currentTransformToReach);
       
        if (Vector3.SqrMagnitude(positionToReachLocal) < acceptableRadius * acceptableRadius)
        {
            Switch();
        }
    }

    private void Switch()
    {
        positionIndex++;
        if (positionIndex >= patrolPositions.Length)
            positionIndex = 0;
        currentTransformToReach = patrolPositions[positionIndex];
    }

    private void InternalSetVelocity()
    {
        Vector3 direction = (currentTransformToReach - patroller.transform.position);
        rigidbody.velocity = direction.normalized * patrolSpeed;
    }
}
