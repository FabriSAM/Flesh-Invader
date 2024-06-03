using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAction : StateAction
{

    private Vector3[] patrolPositions;
    private int positionIndex;
    private float acceptableRadius;

    private Vector3 currentTransformToReach;
    private NavMeshAgent navMeshAgent;

    private float patrolSpeed;

    public PatrolAction(NavMeshAgent patroller, Vector3[] patrolPositions, float acceptableRadius, float patrolSpeed)
    {
        navMeshAgent = patroller;

        this.patrolPositions = patrolPositions;
        this.acceptableRadius = acceptableRadius;
        this.patrolSpeed = patrolSpeed;
    }

    public override void OnEnter()
    {
        if (patrolPositions.Length != 0)
            currentTransformToReach = patrolPositions[0];
        else 
            currentTransformToReach = navMeshAgent.transform.position;
        InternalSetVelocity();
    }

    public override void OnUpdate()
    {
        InternalSetVelocity();
        Vector3 positionToReachLocal = navMeshAgent.transform.InverseTransformPoint(currentTransformToReach);
       
        if (Vector3.SqrMagnitude(positionToReachLocal) < acceptableRadius * acceptableRadius)
        {
            Switch();
        }
    }

    private void Switch()
    {
        navMeshAgent.speed = 0;
        positionIndex++;
        if (positionIndex >= patrolPositions.Length)
            positionIndex = 0;
        currentTransformToReach = patrolPositions[positionIndex];
    }

    private void InternalSetVelocity()
    {
        navMeshAgent.destination = currentTransformToReach;
        navMeshAgent.speed = this.patrolSpeed;
    }
}
