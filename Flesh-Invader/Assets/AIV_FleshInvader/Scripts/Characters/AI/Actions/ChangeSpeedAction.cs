using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeSpeedAction : StateAction
{
    private Rigidbody rigidbody;
    private Vector3 speed;
    private bool everyFrame;


    public ChangeSpeedAction(Rigidbody rigidbody, Vector3 newSpeed, bool everyFrame)
    {
        this.rigidbody = rigidbody;
        this.speed = newSpeed;
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
        rigidbody.velocity = speed;
    }
}
