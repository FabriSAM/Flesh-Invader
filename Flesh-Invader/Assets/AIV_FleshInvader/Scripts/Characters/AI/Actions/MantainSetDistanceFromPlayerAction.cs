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

    private float pathCalculusFrequency;
    private float pathCalculusCounter;


    public MantainSetDistanceFromPlayerAction(NavMeshAgent distancer, float speed, float distanceToReach, float pathCalculusFrequency)
    {
        agent = distancer;
        this.speed = speed;
        this.distanceToReach = distanceToReach;
        this.pathCalculusFrequency = pathCalculusFrequency;
        navPath = new NavMeshPath();
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
        if (agent.CalculatePath(PlayerState.Get().CurrentPlayer.transform.position, navPath) && navPath.status == NavMeshPathStatus.PathComplete)
        {
            agent.destination = PlayerState.Get().CurrentPlayer.transform.position + ((agent.transform.position - PlayerState.Get().CurrentPlayer.transform.position).normalized * distanceToReach);
        }
        agent.speed = speed;
    }
}

