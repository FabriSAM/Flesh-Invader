using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    Controller controller;

    [Header("Speeds")]
    [SerializeField] float baseSpeed;
    [SerializeField] float chaseSpeed;

    protected Transform TargetTransform;
    [SerializeField] protected float distanceToFollowPlayer;
    [SerializeField] protected float distanceToStopFollowPlayer;
    [SerializeField] protected float distanceToStartAttack;
    [SerializeField] protected float distanceToStopAttack;
   
    [Header("Patrol")]
    [SerializeField] protected float patrolAcceptableRadius;
    [SerializeField] protected float patrolPointsGenerationRadius;
    [SerializeField] protected int patrolPointNumber;
    protected Vector3[] PatrolPositions;

    [Header("Stutter")]
    [SerializeField] protected float stutterTime;
    [SerializeField] protected Material testStutterMaterial;

    #region Transition
    private Transition StartChase(State prev, State next)
    {
        Transition transition = new Transition();
        CheckDistanceCondition distanceCondition = new CheckDistanceCondition(transform, PlayerState.Get().PlayerTransform,
            distanceToFollowPlayer, COMPARISON.LESSEQUAL);
        transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
        return transition;
    }

    private Transition StopChase(State prev, State next)
    {
        Transition transition = new Transition();
        CheckDistanceCondition distanceCondition = new CheckDistanceCondition(transform, PlayerState.Get().PlayerTransform,
            distanceToStopFollowPlayer, COMPARISON.GREATEREQUAL);
        transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
        return transition;
    }

    private Transition GoInStutter(State prev, State next)
    {
        Transition transition = new Transition();
        WaitTimeCondition timeCondition = new WaitTimeCondition(stutterTime);
        transition.SetUpMe(prev, next, new Condition[] { timeCondition });
        return transition;
    }
    #endregion


    #region States
    private State SetUpPatrol()
    {
        State patrol = new State();
   
        PatrolPositions = new Vector3[patrolPointNumber];

        //ChangeSpeedAction setPatrolSpeed = new ChangeSpeedAction(GetComponent<Rigidbody>(),new Vector3(baseSpeed,0,0), false);
        GeneratePatrolPointAction generatePatrolPoints = new GeneratePatrolPointAction(transform.position, patrolPointsGenerationRadius, patrolPointNumber, PatrolPositions);
        PatrolAction patrolAction = new PatrolAction(gameObject, PatrolPositions, patrolAcceptableRadius, baseSpeed);

        patrol.SetUpMe(new StateAction[] { /*setPatrolSpeed,*/ generatePatrolPoints, patrolAction });

        return patrol;
    }

    private State SetUpChase()
    {
        State chase = new State();

        ChaseTargetAction chaseTarget = new ChaseTargetAction(gameObject, chaseSpeed);

        chase.SetUpMe(new StateAction[] { chaseTarget });
        return chase;
    }

    private State SetUpStutter()
    {
        State stutter = new State();
        TEST_ChangeMaterialAction changeMaterial = new TEST_ChangeMaterialAction(GetComponent<MeshRenderer>(), testStutterMaterial);
        ChangeSpeedAction stopCharacter = new ChangeSpeedAction(GetComponent<Rigidbody>(), new Vector3(0, 0, 0), false);

        stutter.SetUpMe(new StateAction[] { changeMaterial, stopCharacter });
        return stutter;
    }

    #endregion

    private void Start()
    {
        StateMachine FSM = GetComponentInChildren<StateMachine>();
        State patrol = SetUpPatrol();
        State chase = SetUpChase();
        State stutter = SetUpStutter();
        patrol.SetUpMe(new Transition[] { StartChase(patrol, stutter) });
        stutter.SetUpMe(new Transition[] { GoInStutter(stutter, chase) });
        chase.SetUpMe(new Transition[] {StopChase(chase, patrol) });
        FSM.Init(new State[] {patrol, stutter, chase }, patrol);
    }

 
}
