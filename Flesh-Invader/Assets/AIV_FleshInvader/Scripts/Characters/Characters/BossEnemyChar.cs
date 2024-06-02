using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyChar : EnemyChar
{
    [SerializeField] float XPMultiplier;

    public override void CastAbility()
    {
        //throw new System.NotImplementedException("BOSS: Ability not implemented");
          
    }

    public override void Possess()
    {
        base.Possess();
        PlayerState.Get().GetComponentInChildren<PlayerStateLevel>().SetXPMultiplyer(XPMultiplier);
    }
}
