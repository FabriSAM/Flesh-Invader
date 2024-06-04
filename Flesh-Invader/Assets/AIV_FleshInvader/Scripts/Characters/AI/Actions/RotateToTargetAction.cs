using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTargetAction : StateAction
{
    GameObject rotater, target;

    public RotateToTargetAction(GameObject rotater, GameObject target)
    {
        this.rotater = rotater;
        this.target = target;
    }

    public override void OnUpdate()
    {
        //rotater.transform.rotation = Quaternion.RotateTowards(rotater.transform.rotation, Quaternion);
        //Quaternion.Slerp(rotater.transform.rotation, new Quaternion(, Time.deltaTime))
        //Debug.DrawLine(rotater.transform.position, rotater.transform.position + (rotater.transform.position - target.transform.position * 100), Color.green);
        //rotater.transform.rotation = Quaternion.LookRotation(rotater.transform.position - target.transform.position);
    }
}
