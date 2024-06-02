using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRanged : AbilityBase, IPoolRequester
{

    [SerializeField]
    private PoolData[] bulletsType;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float fireRateo;
    public PoolData[] Datas
    {
        get { return bulletsType; }
    }
    public override void RegisterInput()
    {
        PlayerState.Get().GenericController.Attack += Attack;
    }

    public override void StopAbility()
    {
       
    }

    public override void UnRegisterInput()
    {
        PlayerState.Get().GenericController.Attack += Attack;
    }

    public void Attack()
    {
        if (characterController.IsPossessed)
        {
            IBullet bulletComponent = Pooler.Instance.GetPooledObject(bulletsType[0]).GetComponent<IBullet>();
            if (bulletComponent == null) return;
            bulletComponent.Shoot(transform, bulletSpeed);
        }
        else
        {
            IBullet bulletComponent = Pooler.Instance.GetPooledObject(bulletsType[1]).GetComponent<IBullet>();
            if (bulletComponent == null) return;
            bulletComponent.Shoot(transform, bulletSpeed);
        }
    }
}
