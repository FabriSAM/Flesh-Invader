using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDistanceFromPlayerCondition : Condition
{
    private Transform from;
    private Transform to;
    private float distanceToCheck;
    private COMPARISON comparison;

    public CheckDistanceFromPlayerCondition(Transform from,
        float distanceToCheck, COMPARISON comparison)
    {
        this.from = from;
        this.distanceToCheck = distanceToCheck * distanceToCheck;
        this.comparison = comparison;
    }

    public override bool Validate()
    {
        to = PlayerState.Get().CurrentPlayer.transform;
        return InternalDistanceCompare();
    }

    private bool InternalDistanceCompare()
    {
        float distanceSquared = (from.position - to.position).sqrMagnitude;
        switch (comparison)
        {
            case COMPARISON.EQUAL:
                return distanceSquared == distanceToCheck;
            case COMPARISON.GREATER:
                return distanceSquared > distanceToCheck;
            case COMPARISON.GREATEREQUAL:
                return distanceSquared >= distanceToCheck;
            case COMPARISON.LESS:
                return distanceSquared < distanceToCheck;
            case COMPARISON.LESSEQUAL:
                return distanceSquared <= distanceToCheck;
            default:
                return false;
        }
    }


}
