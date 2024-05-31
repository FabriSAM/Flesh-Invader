using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneratePatrolPointAction : StateAction
{
    private float patrolRadius;
    private Vector3 centerPosition;
    private int patrolPointsNumber;
    private Vector3[] patrolPositionsToFill;

    public GeneratePatrolPointAction(Vector3 centerPosition, float patrolRadius, int patrolPointsNumber, Vector3[] patrolPositions)
    {
        this.centerPosition = centerPosition;
        this.patrolRadius = patrolRadius;
        this.patrolPointsNumber = patrolPointsNumber;
        this.patrolPositionsToFill = patrolPositions;
    }

    public override void OnEnter()
    {
        NavMeshHit hit;     
        for (int i = 0; i<patrolPointsNumber; i++)
        {
            NavMesh.SamplePosition(
                centerPosition + 
                new Vector3
                (
                    Random.Range(-1f,1f), 0, Random.Range(-1f,1f)
                ).normalized * patrolRadius,
                out hit, patrolRadius,1
            );

            patrolPositionsToFill[i] = centerPosition + hit.position;

        }
    }
}
