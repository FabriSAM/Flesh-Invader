using NotserializableEventManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

public abstract class EnemyChar : MonoBehaviour
{
    private const string animIsMovingParamaterString = "IsMoving";
    private const string animXAxisValue = "XAxisValue";
    private const string animZAxisValue = "ZAxisValue";
    protected Controller controller;
    protected NavMeshAgent agent;

    [Header("Stats")]
    [SerializeField] private EnemyStatisticsTemplate characterStartingInfo;
    private StateMachine FSM;
    private EnemyInfo characterCurrentInfo;

    #region ProtectedVariables


    //[Header("Speeds")]
    //protected float baseSpeed;
    //protected float chaseSpeedMultiplier;
    //protected float chaseSpeed;

    //protected Transform TargetTransform;

    //#region PCAVariables
    //protected float patrolAcceptableRadius;
    //protected float patrolPointsGenerationRadius;
    //protected int patrolPointNumber;
    //protected Vector3[] PatrolPositions;

    //protected float distanceToFollowPlayer;
    //protected float distanceToStopFollowPlayer;

    //protected float distanceToStartAttack;
    //protected float distanceToStopAttack;
    //protected float attackDamage;
    //#endregion

    //#region StutterVariables
    //protected float stutterTime;
    //protected Material testStutterMaterial;
    //#endregion

    protected bool IsUnpossessable;



    #endregion

    #region ProtectedProperties
    public EnemyInfo CharacterInfo {get { return characterCurrentInfo; }}
    #endregion

    #region FSM
    #region Transition
        private Transition StartChase(State prev, State next)
        {
            Transition transition = new Transition();
            CheckDistanceCondition distanceCondition = new CheckDistanceCondition(GetComponentInParent<Transform>(), PlayerState.Get().PlayerTransform,
                characterCurrentInfo.CharStatesStats.distanceToFollowPlayer, COMPARISON.LESSEQUAL);
            transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
            return transition;
        }

        private Transition StopChase(State prev, State next)
        {
            Transition transition = new Transition();
            CheckDistanceCondition distanceCondition = new CheckDistanceCondition(GetComponentInParent<Transform>(), PlayerState.Get().PlayerTransform,
                characterCurrentInfo.CharStatesStats.distanceToStopFollowPlayer, COMPARISON.GREATEREQUAL);
            transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
            return transition;
        }

        private Transition StopStutter(State prev, State next)
        {
            Transition transition = new Transition();
            WaitTimeCondition timeCondition = new WaitTimeCondition(characterCurrentInfo.CharStatesStats.stutterTime);
            //SetAnimatorParameterAction setStutterAnimation = new SetAnimatorParameterAction(GetComponentInParent<Animator>(),
            //    new AnimatorParameterStats("GeneralIntParameter", AnimatorParameterType.INTEGER, 5));
            transition.SetUpMe(prev, next, new Condition[] { timeCondition });
            return transition;
        }

        private Transition ChaseToAttack(State prev, State next)
        {
            Transition transition = new Transition();
            CheckDistanceCondition distanceCondition = new CheckDistanceCondition(GetComponentInParent<Transform>(), PlayerState.Get().PlayerTransform,
                characterCurrentInfo.CharStatesStats.distanceToStartAttack, COMPARISON.LESSEQUAL);


            transition.SetUpMe(prev, next, new Condition[] {distanceCondition });
            return transition;
        }

        private Transition AttackBackToChase(State prev, State next)
        {
            Transition transition = new Transition();
            CheckDistanceCondition distanceCondition = new CheckDistanceCondition(GetComponentInParent<Transform>(), PlayerState.Get().PlayerTransform,
                characterCurrentInfo.CharStatesStats.distanceToStopAttack, COMPARISON.GREATEREQUAL);

    
            transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
            return transition;
        }
    #endregion

    #region States
    private State SetUpPatrol()
        {
            State patrol = new State();
   
            Vector3[] PatrolPositions = new Vector3[characterCurrentInfo.CharStatesStats.patrolPointNumber];

            GeneratePatrolPointAction generatePatrolPoints = new GeneratePatrolPointAction(GetComponentInParent<Transform>().position,
                characterCurrentInfo.CharStatesStats.patrolPointsGenerationRadius, characterCurrentInfo.CharStatesStats.patrolPointNumber, PatrolPositions);
            PatrolAction patrolAction = new PatrolAction(agent, PatrolPositions, characterCurrentInfo.CharStatesStats.patrolAcceptableRadius, characterCurrentInfo.CharStats.BaseSpeed);
            AnimatorParameterStats isMoving = new AnimatorParameterStats(animIsMovingParamaterString, AnimatorParameterType.BOOL, true);
            SetAnimatorParameterAction setRunning = new SetAnimatorParameterAction(controller.Visual.CharacterAnimator, isMoving, false);

            AnimatorParameterStats moveAxisX = new AnimatorParameterStats(animXAxisValue, AnimatorParameterType.FLOAT, true);
            AnimatorParameterStats moveAxisZ = new AnimatorParameterStats(animZAxisValue, AnimatorParameterType.FLOAT, true);
            GetSpeedAction animSpeedX = new GetSpeedAction(controller.Visual.CharacterAnimator, agent, VectorAxis.X, moveAxisX, true);
            GetSpeedAction animSpeedZ = new GetSpeedAction(controller.Visual.CharacterAnimator, agent, VectorAxis.Z,  moveAxisZ, true);
            patrol.SetUpMe(new StateAction[] { generatePatrolPoints, patrolAction, setRunning, animSpeedX, animSpeedZ });

            return patrol;
        }

        private State SetUpChase()
        {
            State chase = new State();

            ChaseTargetAction chaseTarget = new ChaseTargetAction(agent, characterCurrentInfo.CharStats.ChaseSpeed);
            AnimatorParameterStats isMoving = new AnimatorParameterStats(animIsMovingParamaterString, AnimatorParameterType.BOOL, true);
            AnimatorParameterStats moveAxisX = new AnimatorParameterStats(animXAxisValue, AnimatorParameterType.FLOAT, true);
            AnimatorParameterStats moveAxisZ = new AnimatorParameterStats(animZAxisValue, AnimatorParameterType.FLOAT, true);
            SetAnimatorParameterAction setRunning = new SetAnimatorParameterAction(controller.Visual.CharacterAnimator, isMoving, false);
            GetSpeedAction animSpeedX = new GetSpeedAction(controller.Visual.CharacterAnimator, agent, VectorAxis.X, moveAxisX,  true);
            GetSpeedAction animSpeedZ = new GetSpeedAction(controller.Visual.CharacterAnimator, agent, VectorAxis.Z, moveAxisZ, true);
            chase.SetUpMe(new StateAction[] { chaseTarget, setRunning, animSpeedX, animSpeedZ });
            return chase;
        }

        private State SetUpStutter()
        {
            State stutter = new State();
            TEST_ChangeMaterialAction changeMaterial = new TEST_ChangeMaterialAction(GetComponentInParent<MeshRenderer>(), characterCurrentInfo.CharStatesStats.testStutterMaterial);
            ChangeSpeedAction stopCharacter = new ChangeSpeedAction(agent, 0, false);
            AnimatorParameterStats isMoving = new AnimatorParameterStats(animIsMovingParamaterString, AnimatorParameterType.BOOL, false);
            SetAnimatorParameterAction setRunning = new SetAnimatorParameterAction(controller.Visual.CharacterAnimator, isMoving, false);

            AnimatorParameterStats moveAxisX = new AnimatorParameterStats(animXAxisValue, AnimatorParameterType.FLOAT, true);
            AnimatorParameterStats moveAxisZ = new AnimatorParameterStats(animZAxisValue, AnimatorParameterType.FLOAT, true);
            GetSpeedAction animSpeedX = new GetSpeedAction(controller.Visual.CharacterAnimator, agent, VectorAxis.X, moveAxisX, true);
            GetSpeedAction animSpeedZ = new GetSpeedAction(controller.Visual.CharacterAnimator, agent, VectorAxis.Z, moveAxisZ, true);
        stutter.SetUpMe(new StateAction[] { changeMaterial, stopCharacter });
            return stutter;
        }
        private State SetUpAttack()
        {
            State attack = new State();
            AIAttackAction attackTarget = new AIAttackAction(controller, characterCurrentInfo.CharStats.AttackCountdown, false);

            attack.SetUpMe(new StateAction[] { attackTarget });
            return attack;
        }

        #endregion

    #endregion

    #region Mono
    private void Awake()
    {
        InitializeEnemy();
    }

    private void OnEnable()
    {
        FSM = GetComponentInChildren<StateMachine>();
        agent = GetComponentInParent<NavMeshAgent>();
        
        CharacterStatsConfiguration();

        State patrol = SetUpPatrol();
        State chase = SetUpChase();
        State stutter = SetUpStutter();
        State attack = SetUpAttack();
        patrol.SetUpMe(new Transition[] { StartChase(patrol, stutter) });
        stutter.SetUpMe(new Transition[] { StopStutter(stutter, chase) });
        chase.SetUpMe(new Transition[] {StopChase(chase, patrol), ChaseToAttack(chase,attack) });
        attack.SetUpMe(new Transition[] { AttackBackToChase(attack,chase)});

        FSM.Init(new State[] {patrol, stutter, chase, attack }, patrol);
    }


    #endregion

    #region Initialization

    protected virtual void InitializeEnemy()
    {
        controller = GetComponentInParent<Controller>();
        controller.OnCharacterPossessed += InternalPossess;
        controller.OnCharacterUnpossessed += InternalUnPossess;

        characterCurrentInfo = new EnemyInfo();

        // Delegate bounding
        controller.attack += CastAbility;
    }

    private void CharacterStatsConfiguration()
    {
        // To change respecting random multipliers
        characterCurrentInfo.CharStats = characterStartingInfo.CharInfo.CharStats;
        characterCurrentInfo.CharStatesStats = characterStartingInfo.CharInfo.CharStatesStats;
        characterCurrentInfo.CharNarrativeStats = characterStartingInfo.CharInfo.CharNarrativeStats;


        CalculateDamage();

        // Unpossessable EnemyChar behavior
        ObjectByProbability<bool> unpossessableProb = new ObjectByProbability<bool>();
        unpossessableProb.Max = characterStartingInfo.CharStats.UnpossessableProbability;

        IsUnpossessable = unpossessableProb.IsInRange(UnityEngine.Random.Range(0f, 1f));
        if (IsUnpossessable)
        {
            Debug.Log("Spawn unposessable enemyChar");

            // Unpossessable Enemy differences with normal enemy
            transform.parent.localScale *= 3;
            characterCurrentInfo.CharStatesStats.patrolPointsGenerationRadius *= 3;
        }
    }

    private void CalculateDamage()
    {
        int playerLevel = PlayerState.Get().GetComponentInChildren<PlayerStateLevel>().GetXP();
        characterCurrentInfo.CharStats.Damage *= UnityEngine.Random.Range(CharacterInfo.CharStats.MinDamageMultiplier, CharacterInfo.CharStats.MaxDamageMultiplier) * playerLevel;
    }
    #endregion

    #region Enemy
    public abstract void CastAbility();

    public virtual void InternalPossess()
    {
        if (IsUnpossessable) return;

        agent.enabled = false;


        GlobalEventSystem.CastEvent(
            EventName.PossessionExecuted, 
            EventArgsFactory.PossessionExecutedFactory(CharacterInfo)
            );
        FSM.enabled = false;
    }

    public virtual void InternalUnPossess()
    {
        FSM.enabled = true;
        agent.enabled = true;
    }
    #endregion
}
