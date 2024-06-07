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
        //rotater.transform.rotation = Quaternion.RotateTowards(rotater.transform.rotation, Quaternion);
        //Quaternion.Slerp(rotater.transform.rotation, new Quaternion(, Time.deltaTime))
        //Debug.DrawLine(rotater.transform.position, rotater.transform.position + (rotater.transform.position - target.transform.position * 100), Color.green);
        //rotater.transform.rotation = Quaternion.LookRotation(rotater.transform.position - target.transform.position);
        rotater.transform.rotation = Quaternion.LookRotation(PlayerState.Get().PlayerTransform.position - rotater.transform.position);
    }
}
