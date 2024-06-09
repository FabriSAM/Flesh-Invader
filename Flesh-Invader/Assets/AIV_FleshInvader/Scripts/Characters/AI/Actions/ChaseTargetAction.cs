using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseTargetAction : StateAction
{
    private NavMeshAgent chaserAgent;
    private float chaseSpeed;

    public ChaseTargetAction(NavMeshAgent chaser, float chaseSpeed)
    {
        chaserAgent = chaser;
        this.chaseSpeed = chaseSpeed;
    }

    public override void OnUpdate()
    {
        chaserAgent.destination = PlayerState.Get().CurrentPlayer.transform.position;
        chaserAgent.speed = chaseSpeed;
    }
}
