using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : AbilityBase, IPoolRequester
{
    [SerializeField]
    private PoolData[] bulletsType;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private Transform aim;


    public PoolData[] Datas
    {
        get { return bulletsType; }
    }

    public override void RegisterInput()
    {
        PlayerState.Get().GenericController.Posses += ShootPossession;
        PlayerState.Get().GenericController.Pos2 += UnRegisterInput;
    }

    public override void StopAbility()
    {
       
    }

    public override void UnRegisterInput()
    {
        PlayerState.Get().GenericController.Posses -= ShootPossession;
    }

    private void ShootPossession()
    {
        IBullet bulletComponent = Pooler.Instance.GetPooledObject(bulletsType[0]).GetComponent<IBullet>();
        if (bulletComponent == null) return;
        bulletComponent.Shoot(aim, bulletSpeed, characterController);
    }
}
