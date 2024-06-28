using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AbilityBase
{
    [SerializeField]
    private float meleeCD;

    private const string MeleeTriggerAnimatorParameter = "AttackTrigger";
    private float currentTimeCache;

    #region FMOD
    private const string stabbedEventName = "Stabbed";
    private const string stabbedBankName = "Enemies";
    #endregion

    public override void Init(Controller characterController)
    {
        base.Init(characterController);
        currentTimeCache = Time.time - meleeCD;
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
        PlayerState.Get().GenericController.Attack -= Attack;
    }

    public void Attack()
    {
        if (Time.time-currentTimeCache>meleeCD)
        {
            currentTimeCache=Time.time;
            characterController.Visual.SetAnimatorParameter(MeleeTriggerAnimatorParameter);
            if (characterController.IsPossessed) {
                AudioManager.Get().PlayOneShot(stabbedEventName, stabbedBankName);
            }
        }
    }
}
