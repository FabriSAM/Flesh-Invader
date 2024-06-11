using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForFreeAttackPositionCondition : Condition
{
    private Transform attacker;
    private bool freePos;
    private EnvTargetPoint target;

    public CheckForFreeAttackPositionCondition(Transform attacker)
    {
        this.attacker = attacker;
    }

    public override bool Validate()
    {
        IAController.CheckNearestTarget(attacker.position, out freePos, out target);
        if (freePos)
        {
            target.IsOccupied = true;
            return true;
        }

        return false;
    }

}
