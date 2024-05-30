using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTargetAction : StateAction
{
    private Rigidbody chaserRigidbody;
    private float chaseSpeed;

    public ChaseTargetAction(GameObject chaser, float chaseSpeed)
    {
        chaserRigidbody = chaser.GetComponent<Rigidbody>();
        this.chaseSpeed = chaseSpeed;
    }

    public override void OnUpdate()
    {
        chaserRigidbody.velocity = (PlayerState.Get().PlayerTransform.position - chaserRigidbody.position).normalized * chaseSpeed;
    }
}
