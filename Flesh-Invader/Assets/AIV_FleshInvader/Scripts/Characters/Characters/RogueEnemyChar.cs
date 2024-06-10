using Codice.Client.BaseCommands.Import;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static PlasticPipe.Server.MonitorStats;
using UnityEngine.AI;

public class RogueEnemyChar : EnemyChar
{
    protected State combat;
    protected bool attackAnimationEnded;    // UNUSED FOR NOW

    public override void CastAbility()
    {
        //throw new System.NotImplementedException("ROGUE: Ability not implemented");
    }

    #region CombatStateConfiguration
    //protected override void InitFSM()
    //{
    //    patrol = SetUpPatrol();
    //    chase = SetUpChase();
    //    stutter = SetUpStutter();
    //    combat = SetUpCombat();
    //    attack = SetUpAttack();

    //    dying = SetUpDying();
    //    death = SetUpDeath();

    //    patrol.SetUpMe(new Transition[] { StartChase(patrol, stutter) });
    //    stutter.SetUpMe(new Transition[] { StopStutter(stutter, chase) });
    //    chase.SetUpMe(new Transition[] { StopChase(chase, patrol), ChaseToAttack(chase, attack) });
    //    combat.SetUpMe(new Transition[] { CombatBackToChase(combat, chase), CombatToAttack(combat, attack) });
    //    attack.SetUpMe(new Transition[] { AttackBackToCombat(attack, combat) });

    //    //dying.SetUpMe(new Transition[] { DyingToDeath()});


    //    FSM.Init(new State[] { patrol, stutter, chase, combat, attack }, patrol);
    //}

    //#region FSM


    //protected State SetUpCombat()
    //{
    //    State attack = new State();
    //    MantainSetDistanceFromPlayerAction MantainSetDistance = new MantainSetDistanceFromPlayerAction(agent, characterCurrentInfo.CharStats.BaseSpeed, characterCurrentInfo.CharStatesStats.distanceToStartCombat - 0.1f);

    //    AnimatorParameterStats isAttacking = new AnimatorParameterStats(animAttackString, AnimatorParameterType.TRIGGER, true);
    //    SetAnimatorParameterAction setAttackingAnim = new SetAnimatorParameterAction(controller.Visual.CharacterAnimator, isAttacking,
    //        true, characterCurrentInfo.CharStats.AttackCountdown);
    //    RotateToPlayerAction rotateToTarget = new RotateToPlayerAction(agent);

    //    attack.SetUpMe(new StateAction[] { MantainSetDistance, setAttackingAnim, rotateToTarget, });
    //    return attack;
    //}

    //#region Transitions
    //protected Transition CombatBackToChase(State prev, State next)
    //{
    //    Transition transition = new Transition();
    //    CheckDistanceCondition distanceCondition = new CheckDistanceCondition(GetComponentInParent<Transform>(), PlayerState.Get().CurrentPlayer.transform,
    //        characterCurrentInfo.CharStatesStats.distanceToStopAttack, COMPARISON.GREATEREQUAL);


    //    transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
    //    return transition;
    //}

    //protected Transition CombatToAttack(State prev, State next)
    //{
    //    Transition transition = new Transition();
    //    CheckForFreeAttackPositionCondition checkForFreeAttackPlace = new CheckForFreeAttackPositionCondition(GetComponentInParent<Transform>());


    //    transition.SetUpMe(prev, next, new Condition[] { checkForFreeAttackPlace });
    //    return transition;
    //}

    //protected Transition AttackBackToCombat(State prev, State next)
    //{
    //    Transition transition = new Transition();

    //    //FOR TESTING
    //    //TO DO: REPLACE WITH ATTACKANIMATIONEND
    //    WaitTimeCondition timeCondition = new WaitTimeCondition(2);

    //    transition.SetUpMe(prev, next, new Condition[] { timeCondition });
    //    return transition;
    //}
    //#endregion

    #endregion
}
