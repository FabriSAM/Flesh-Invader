using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AbilityBase
{
    [SerializeField]
    private float meleeCD;

    private const string MeleeTriggerAnimatorParameter = "AttackTrigger";
    private float currentTimeCache;

    public override void Init(Controller characterController)
    {
        base.Init(characterController);
        currentTimeCache = Time.time - meleeCD;
    }
    public override void RegisterInput()
    {
        PlayerState.Get().GenericController.Pos2 += UnRegisterInput;
        PlayerState.Get().GenericController.Attack += Attack;
    }

    public override void StopAbility()
    {
       
    }

    public override void UnRegisterInput()
    {
        PlayerState.Get().GenericController.Attack -= Attack;
    }

    public void Attack()
    {
        if (Time.time-currentTimeCache>meleeCD)
        {
            currentTimeCache=Time.time;
            characterController.Visual.SetAnimatorParameter(MeleeTriggerAnimatorParameter);
        }
    }
}
