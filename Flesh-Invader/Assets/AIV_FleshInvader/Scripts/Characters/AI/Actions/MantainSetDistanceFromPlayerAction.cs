using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MantainSetDistanceFromPlayerAction : StateAction
{
    private NavMeshAgent agent;
    private NavMeshPath navPath;
    private float speed;
    private float distanceToReach;

    public MantainSetDistanceFromPlayerAction(NavMeshAgent distancer, float speed, float distanceToReach)
    {
        agent = distancer;
        this.speed = speed;
        this.distanceToReach = distanceToReach;
        navPath = new NavMeshPath();
    }

    public override void OnUpdate()
    {
        if (agent.CalculatePath(PlayerState.Get().PlayerTransform.position, navPath) && navPath.status == NavMeshPathStatus.PathComplete)
        {
            agent.destination = PlayerState.Get().PlayerTransform.position + ((agent.transform.position - PlayerState.Get().PlayerTransform.position).normalized * distanceToReach);
        }
        agent.speed = speed;
    }
}
