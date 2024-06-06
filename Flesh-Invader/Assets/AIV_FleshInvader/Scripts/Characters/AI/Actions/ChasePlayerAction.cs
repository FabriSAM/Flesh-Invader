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

    public ChasePlayerAction(NavMeshAgent chaser, float chaseSpeed, float distanceToReach)
    {
        chaserAgent = chaser;
        this.chaseSpeed = chaseSpeed;
        this.distanceToReach = distanceToReach;
        navPath = new NavMeshPath();
    }

    public override void OnUpdate()
    {
        if (chaserAgent.CalculatePath(PlayerState.Get().PlayerTransform.position, navPath) && navPath.status == NavMeshPathStatus.PathComplete)
        {
            chaserAgent.destination = PlayerState.Get().PlayerTransform.position + ((chaserAgent.transform.position -  PlayerState.Get().PlayerTransform.position).normalized * distanceToReach);
        }
        chaserAgent.speed = chaseSpeed;
    }
}
