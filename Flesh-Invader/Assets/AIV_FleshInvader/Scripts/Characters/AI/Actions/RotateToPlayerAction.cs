using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotateToPlayerAction : StateAction
{
    NavMeshAgent rotater;

    public RotateToPlayerAction(NavMeshAgent rotater)
    {
        this.rotater = rotater;
    }

    public override void OnEnter()
    {
        rotater.updateRotation = false;
    }

    public override void OnExit()
    {
        rotater.updateRotation = true;
    }

    public override void OnUpdate()
    {
        rotater.transform.rotation = Quaternion.LookRotation(PlayerState.Get().CurrentPlayer.transform.position - rotater.transform.position);
    }
}
