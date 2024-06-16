using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyChar : EnemyChar
{

    [SerializeField] protected float XPMultiplier;
    [SerializeField] protected Transform aimTransform;
    protected AttackRanged RangedAttackComponent;

    private void Awake()
    {
        RangedAttackComponent = GetComponentInChildren<AttackRanged>();
        InitializeEnemy();
    }

    public override void CastAbility()
    {
        RangedAttackComponent.Attack();
    }

    public override void InternalPossess()
    {
        base.InternalPossess();
        PlayerState.Get().LevelController.SetXPMultiplyer(XPMultiplier);
    }

    public override void InternalUnPossess()
    {
        base.InternalUnPossess();
        PlayerState.Get().LevelController.SetXPMultiplyer(1);
    }

}
