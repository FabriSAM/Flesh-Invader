using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeSpeedAction : StateAction
{
    private NavMeshAgent agent;
    private float speed;
    private bool everyFrame;


    public ChangeSpeedAction(NavMeshAgent agent, float speed, bool everyFrame)
    {
        this.agent = agent;
        this.speed = speed;
        this.everyFrame = everyFrame;
    }

    public override void OnEnter()
    {
        InternalSetVelocity();
    }

    public override void OnUpdate()
    {
        if (!everyFrame) return;
        InternalSetVelocity();
    }

    private void InternalSetVelocity()
    {
        agent.speed = speed;
    }
}
