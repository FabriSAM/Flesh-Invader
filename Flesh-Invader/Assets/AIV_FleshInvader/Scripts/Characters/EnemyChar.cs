using NotserializableEventManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyChar : MonoBehaviour, IPossessable
{
    protected Controller controller;
    protected NavMeshAgent agent;

    [Header("Stats")]
    [SerializeField] private EnemyStatisticsTemplate characterStartingInfo;

    #region ProtectedVariables
    [Header("Speeds")]
    protected float baseSpeed;
    protected float chaseSpeedMultiplier;
    protected float chaseSpeed;


    protected Transform TargetTransform;
    protected float distanceToFollowPlayer;
    protected float distanceToStopFollowPlayer;
    protected float distanceToStartAttack;
    protected float distanceToStopAttack;

    protected float patrolAcceptableRadius;
    protected float patrolPointsGenerationRadius;
    protected int patrolPointNumber;
    protected Vector3[] PatrolPositions;

    [Header("Stutter")]
    [SerializeField] protected float stutterTime;
    [SerializeField] protected Material testStutterMaterial;

    #endregion

    #region ProtectedProperties
    public EnemyInfo CharacterInfo {get { return characterStartingInfo.CharInfo; }}
    #endregion

    #region FSM
    #region Transition
    private Transition StartChase(State prev, State next)
    {
        Transition transition = new Transition();
        CheckDistanceCondition distanceCondition = new CheckDistanceCondition(GetComponentInParent<Transform>(), PlayerState.Get().PlayerTransform,
            distanceToFollowPlayer, COMPARISON.LESSEQUAL);
        transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
        return transition;
    }

    private Transition StopChase(State prev, State next)
    {
        Transition transition = new Transition();
        CheckDistanceCondition distanceCondition = new CheckDistanceCondition(GetComponentInParent<Transform>(), PlayerState.Get().PlayerTransform,
            distanceToStopFollowPlayer, COMPARISON.GREATEREQUAL);
        transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
        return transition;
    }

    private Transition GoInStutter(State prev, State next)
    {
        Transition transition = new Transition();
        WaitTimeCondition timeCondition = new WaitTimeCondition(stutterTime);
        //SetAnimatorParameterAction setStutterAnimation = new SetAnimatorParameterAction(GetComponentInParent<Animator>(),
        //    new AnimatorParameterStats("GeneralIntParameter", AnimatorParameterType.INTEGER, 5));
        transition.SetUpMe(prev, next, new Condition[] { timeCondition });
        return transition;
    }
    #endregion

    #region States
    private State SetUpPatrol()
    {
        State patrol = new State();
   
        PatrolPositions = new Vector3[patrolPointNumber];

        GeneratePatrolPointAction generatePatrolPoints = new GeneratePatrolPointAction(GetComponentInParent<Transform>().position, 
            patrolPointsGenerationRadius, patrolPointNumber, PatrolPositions);
        PatrolAction patrolAction = new PatrolAction(agent, PatrolPositions, patrolAcceptableRadius, baseSpeed);

        patrol.SetUpMe(new StateAction[] { generatePatrolPoints, patrolAction });

        return patrol;
    }

    private State SetUpChase()
    {
        State chase = new State();

        ChaseTargetAction chaseTarget = new ChaseTargetAction(agent, chaseSpeed);

        chase.SetUpMe(new StateAction[] { chaseTarget });
        return chase;
    }

    private State SetUpStutter()
    {
        State stutter = new State();
        TEST_ChangeMaterialAction changeMaterial = new TEST_ChangeMaterialAction(GetComponentInParent<MeshRenderer>(), testStutterMaterial);
        ChangeSpeedAction stopCharacter = new ChangeSpeedAction(agent, 0, false);

        stutter.SetUpMe(new StateAction[] { changeMaterial, stopCharacter });
        return stutter;
    }

    #endregion

    private void CharacterStatsConfiguration()
    {
        // To change respecting random multipliers
        baseSpeed = characterStartingInfo.Speed;
        chaseSpeedMultiplier = characterStartingInfo.ChaseSpeedMultiplier;
        chaseSpeed = characterStartingInfo.Speed * chaseSpeedMultiplier;

        distanceToFollowPlayer = characterStartingInfo.DistanceToFollowPlayer;
        distanceToStopFollowPlayer = characterStartingInfo.DistanceToStopFollowPlayer;
        distanceToStartAttack = characterStartingInfo.DistanceToStartAttack;
        distanceToStopAttack = characterStartingInfo.DistanceToStopAttack;

        patrolAcceptableRadius = characterStartingInfo.PatrolAcceptableRadius;
        patrolPointsGenerationRadius = characterStartingInfo.PatrolPointsGenerationRadius;
        patrolPointNumber = characterStartingInfo.PatrolPointNumber;

    }
    #endregion

    #region Mono
    private void Awake()
    {
        controller = GetComponentInParent<Controller>();
    }

    private void OnEnable()
    {
        StateMachine FSM = GetComponentInChildren<StateMachine>();
        agent = GetComponentInParent<NavMeshAgent>();
        
        CharacterStatsConfiguration();

        State patrol = SetUpPatrol();
        State chase = SetUpChase();
        State stutter = SetUpStutter();
        patrol.SetUpMe(new Transition[] { StartChase(patrol, stutter) });
        stutter.SetUpMe(new Transition[] { GoInStutter(stutter, chase) });
        chase.SetUpMe(new Transition[] {StopChase(chase, patrol) });
        FSM.Init(new State[] {patrol, stutter, chase }, patrol);
    }

    #endregion

    public abstract void CastAbility();

    public void Possess()
    {
        controller.InternalOnPosses();
        GlobalEventSystem.CastEvent(
            EventName.PossessionExecuted, 
            EventArgsFactory.PossessionExecutedFactory(CharacterInfo)
            );
        enabled = false;
    }

    public void UnPossess()
    {
        controller.InternalOnUnposses();

    }
}
