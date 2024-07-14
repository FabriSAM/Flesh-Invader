using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTimeCondition : Condition
{
    private float elapsedTime;
    private float timeToWait;

    public WaitTimeCondition(float timeToWait)
    {
        this.timeToWait = timeToWait;
    }

    public override void OnEnter()
    {
        elapsedTime = 0;
    }

    public override bool Validate()
    {
        return InternalTimePassedCheck();
    }

    private bool InternalTimePassedCheck()
    {
        elapsedTime += Time.deltaTime;
        return elapsedTime > timeToWait;
    }
}
