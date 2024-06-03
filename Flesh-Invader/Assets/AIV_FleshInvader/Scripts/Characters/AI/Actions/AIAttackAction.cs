using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackAction : StateAction
{
    Controller AIController;
    float attackCountdown;
    float attackCountdownCounter;
    bool isOneShot;

    public AIAttackAction(Controller AIController, float AttackCountdown, bool isOneShot)
    {
        this.AIController = AIController;
        this.attackCountdown = AttackCountdown;
        this.isOneShot = isOneShot;
    }

    public override void OnEnter()
    {
        AIController.attack?.Invoke();
    }

    public override void OnUpdate()
    {
        if(!isOneShot)
        {
            attackCountdownCounter += Time.deltaTime;
            if (attackCountdownCounter >= attackCountdown)
            {
                AIController.attack?.Invoke();
                attackCountdownCounter = 0;
            }
        }
    }
}
