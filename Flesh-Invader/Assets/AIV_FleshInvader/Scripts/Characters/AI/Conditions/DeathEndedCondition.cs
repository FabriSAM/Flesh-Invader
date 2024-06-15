using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DeathEndedCondition : Condition
{
    private EnemyChar enemy;

    public DeathEndedCondition(EnemyChar enemy)
    {
        this.enemy = enemy;
    }

    public override bool Validate()
    {
        return enemy.IsDead;
    }
}