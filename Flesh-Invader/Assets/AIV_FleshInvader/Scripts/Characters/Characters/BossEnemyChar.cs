using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyChar : EnemyChar/*, IPoolRequester*/
{
    //[SerializeField] protected PoolData[] bulletPool;
    [SerializeField] protected float XPMultiplier;
    [SerializeField] protected Transform aimTransform;
    protected AttackRanged RangedAttackComponent;

    //public PoolData[] Datas { get { return bulletPool; } }

    private void Awake()
    {
        //foreach (PoolData pool in bulletPool)
        //{
        //    Pooler.Instance.AddToPool(pool);
        //}

        RangedAttackComponent = GetComponentInChildren<AttackRanged>();
        InitializeEnemy();
    }

    public override void CastAbility()
    {
        //IBullet bulletComponent = Pooler.Instance.GetPooledObject(Datas[0]).GetComponent<IBullet>();
        //if (bulletComponent == null) return;
        //bulletComponent.Shoot(aimTransform, shootSpeed);

        RangedAttackComponent.Attack();

        //RangedAttackComponent

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
