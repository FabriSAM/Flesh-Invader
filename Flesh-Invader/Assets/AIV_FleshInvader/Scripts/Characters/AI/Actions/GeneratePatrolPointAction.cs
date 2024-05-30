using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        for (int i = 0; i<patrolPointsNumber; i++)
        {
            patrolPositionsToFill[i] = centerPosition + new Vector3(
                Random.Range(-patrolRadius, patrolRadius), 
                0,
                Random.Range(-patrolRadius, patrolRadius));

        }
    }
}
