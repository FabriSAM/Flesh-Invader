using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : AbilityBase
{

    public override void RegisterInput()
    {
        PlayerState.Get().GenericController.Posses += ShootPossession;
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
        Debug.Log("ShootPossession");
    }
}
