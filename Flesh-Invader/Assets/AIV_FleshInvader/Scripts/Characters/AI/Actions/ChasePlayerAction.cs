using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerAction : StateAction
{
    private NavMeshAgent chaserAgent;
    private NavMeshPath navPath;
    private float chaseSpeed;
    private float distanceToReach;

    private float pathCalculusFrequency;
    private float pathCalculusCounter;

    public ChasePlayerAction(NavMeshAgent chaser, float chaseSpeed, float distanceToReach, float pathCalculusFrequency)
    {
        chaserAgent = chaser;
        this.chaseSpeed = chaseSpeed;
        this.distanceToReach = distanceToReach;
        this.pathCalculusFrequency = pathCalculusFrequency;

        navPath = new NavMeshPath();
    }

    public override void OnEnter()
    {
        InternalSetVelocity();
    }

    public override void OnUpdate()
    {
        pathCalculusCounter += Time.deltaTime;

        if (pathCalculusCounter >= pathCalculusFrequency)
        {
            InternalSetVelocity();
            pathCalculusCounter = 0;
        }

    }

    private void InternalSetVelocity()
    {
        if (chaserAgent.gameObject.activeSelf && chaserAgent.CalculatePath(PlayerState.Get().CurrentPlayer.transform.position, navPath) && navPath.status == NavMeshPathStatus.PathComplete)
        {
            chaserAgent.destination = PlayerState.Get().CurrentPlayer.transform.position + ((chaserAgent.transform.position - PlayerState.Get().CurrentPlayer.transform.position).normalized * distanceToReach);
        }
        chaserAgent.speed = chaseSpeed;
    }
}
