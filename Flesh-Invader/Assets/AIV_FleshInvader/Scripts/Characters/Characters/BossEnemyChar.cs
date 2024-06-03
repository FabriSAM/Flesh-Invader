using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyChar : EnemyChar, IPoolRequester
{
    [SerializeField] protected PoolData[] bulletPool;
    [SerializeField] protected int TEST_bulletType;
    [SerializeField] protected float XPMultiplier;
    [SerializeField] protected float shootSpeed;

    public PoolData[] Datas { get { return bulletPool; } }

    private void Awake()
    {
        foreach (PoolData pool in bulletPool)
        {
            Pooler.Instance.AddToPool(pool);
        }

        InitializeEnemy();
    }

    public override void CastAbility()
    {
        IBullet bulletComponent = Pooler.Instance.GetPooledObject(Datas[0]).GetComponent<IBullet>();
        if (bulletComponent == null) return;
        bulletComponent.Shoot(transform, shootSpeed);

        //RangedAttackComponent
    }

    public override void Possess()
    {
        base.Possess();
        PlayerState.Get().GetComponentInChildren<PlayerStateLevel>().SetXPMultiplyer(XPMultiplier);
        //TEST_bulletType = 2 //2 is player bullet type
        //OR
        //RangedAttackComponent
    }

    public override void UnPossess()
    {
        base.UnPossess();
        PlayerState.Get().GetComponentInChildren<PlayerStateLevel>().SetXPMultiplyer(1);
    }

}
