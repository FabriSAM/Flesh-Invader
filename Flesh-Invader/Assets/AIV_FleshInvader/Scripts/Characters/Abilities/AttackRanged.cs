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

    private float currentTimeCache;
    private const string rangedTriggerAnimatorParameter = "AttackTrigger";
    public PoolData[] Datas
    {
        get { return bulletsType; }
    }
    public override void RegisterInput()
    {
        PlayerState.Get().GenericController.Attack += Attack;
        PlayerState.Get().GenericController.Pos2 += UnRegisterInput;
    }

    public override void StopAbility()
    {
       
    }

    public override void UnRegisterInput()
    {
        PlayerState.Get().GenericController.Attack -= Attack;
    }

    public override void Init(Controller characterController)
    {
        base.Init(characterController);
        currentTimeCache = Time.time - fireRateo;
    }

    public void Attack()
    {
        if (characterController.IsPossessed)
        {
            if (Time.time - currentTimeCache > fireRateo)
            {
                currentTimeCache = Time.time;
                IBullet bulletComponent = Pooler.Instance.GetPooledObject(bulletsType[0]).GetComponent<IBullet>();
                if (bulletComponent == null) return;
                bulletComponent.Shoot(transform, bulletSpeed, characterController);
                characterController.Visual.SetAnimatorParameter(rangedTriggerAnimatorParameter);
            }
        }
        else
        {
            IBullet bulletComponent = Pooler.Instance.GetPooledObject(bulletsType[1]).GetComponent<IBullet>();
            if (bulletComponent == null) return;
            bulletComponent.Shoot(transform, bulletSpeed, characterController);
        }
    }
}
