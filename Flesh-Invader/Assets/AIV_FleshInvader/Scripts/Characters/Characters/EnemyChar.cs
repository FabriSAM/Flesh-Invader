using NotserializableEventManager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyChar : MonoBehaviour
{

    protected Controller controller;
    protected NavMeshAgent agent;
    [SerializeField] protected float calculusFrequency;

    [Header("Stats")]
    [SerializeField] protected EnemyStatisticsTemplate characterStartingInfo;
    protected StateMachine FSM;
    protected EnemyInfo characterCurrentInfo;
    private bool isDead;

    [SerializeField] protected Material unpossesableMaterial;

    #region ProtectedProperties
    public EnemyInfo CharacterInfo {get { return characterCurrentInfo; }}

    public bool IsDead { get => isDead; }
    #endregion

    #region AnimatorStrings
    protected const string animIsMovingParamaterString = "IsMoving";
    protected const string animXAxisValue = "XAxisValue";
    protected const string animZAxisValue = "ZAxisValue";
    protected const string animAttackString = "AttackTrigger";
    protected const string animPoseString = "CurrentWeaponType";
    protected const string animDeathString = "Dead";
    #endregion

    #region States
    protected State moving;

    protected State patrol;
    protected State chase;
    protected State stutter;
    protected State attack;
    protected State dying;

    #endregion

    //TO DO
    //MANAGE THE CHANGE OF PLAYER GAMEOBJECT THROUGH EVENTS

    #region FSM
    #region Transition
        protected Transition StartChase(State prev, State next)
        {
            Transition transition = new Transition();
            CheckDistanceFromPlayerCondition distanceCondition = new CheckDistanceFromPlayerCondition(GetComponentInParent<Transform>(), 
                characterCurrentInfo.CharStatesStats.distanceToFollowPlayer, COMPARISON.LESSEQUAL);
            transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
            return transition;
        }

        protected Transition StopChase(State prev, State next)
        {
            Transition transition = new Transition();
            CheckDistanceFromPlayerCondition distanceCondition = new CheckDistanceFromPlayerCondition(GetComponentInParent<Transform>(), 
                characterCurrentInfo.CharStatesStats.distanceToStopFollowPlayer, COMPARISON.GREATEREQUAL);
            transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
            return transition;
        }

        protected Transition StopStutter(State prev, State next)
        {
            Transition transition = new Transition();
            WaitTimeCondition timeCondition = new WaitTimeCondition(characterCurrentInfo.CharStatesStats.stutterTime);
            //SetAnimatorParameterAction setStutterAnimation = new SetAnimatorParameterAction(GetComponentInParent<Animator>(),
            //    new AnimatorParameterStats("GeneralIntParameter", AnimatorParameterType.INTEGER, 5));
            transition.SetUpMe(prev, next, new Condition[] { timeCondition });
            return transition;
        }

        protected Transition ChaseToAttack(State prev, State next)
        {
            Transition transition = new Transition();
            CheckDistanceFromPlayerCondition distanceCondition = new CheckDistanceFromPlayerCondition(GetComponentInParent<Transform>(),
                characterCurrentInfo.CharStatesStats.distanceToStartAttack, COMPARISON.LESSEQUAL);


            transition.SetUpMe(prev, next, new Condition[] {distanceCondition });
            return transition;
        }

        protected Transition AttackBackToChase(State prev, State next)
        {
            Transition transition = new Transition();

            CheckDistanceFromPlayerCondition distanceCondition = new CheckDistanceFromPlayerCondition(GetComponentInParent<Transform>(),
                characterCurrentInfo.CharStatesStats.distanceToStopAttack, COMPARISON.GREATEREQUAL);


            transition.SetUpMe(prev, next, new Condition[] { distanceCondition });
            return transition;
        }

    #endregion

    #region States
    protected virtual State SetUpPatrol()
        {
            State patrol = new State();
   
            Vector3[] PatrolPositions = new Vector3[characterCurrentInfo.CharStatesStats.patrolPointNumber];

            GeneratePatrolPointAction generatePatrolPoints = new GeneratePatrolPointAction(GetComponentInParent<Transform>().position,
                characterCurrentInfo.CharStatesStats.patrolPointsGenerationRadius, characterCurrentInfo.CharStatesStats.patrolPointNumber, PatrolPositions);
            PatrolAction patrolAction = new PatrolAction(agent, PatrolPositions, characterCurrentInfo.CharStatesStats.patrolAcceptableRadius, characterCurrentInfo.CharStats.BaseSpeed, calculusFrequency);
            AnimatorParameterStats isMoving = new AnimatorParameterStats(animIsMovingParamaterString, AnimatorParameterType.BOOL, true);
            SetAnimatorParameterAction setRunning = new SetAnimatorParameterAction(controller.Visual.CharacterAnimator, isMoving, false);

            patrol.SetUpMe(new StateAction[] { generatePatrolPoints, patrolAction, setRunning });

            return patrol;
        }

        protected virtual State SetUpChase()
        {
            State chase = new State();

            ChasePlayerAction chaseTarget = new ChasePlayerAction(agent, characterCurrentInfo.CharStats.ChaseSpeed, characterCurrentInfo.CharStatesStats.distanceToStartAttack-0.1f, calculusFrequency);
            AnimatorParameterStats isMoving = new AnimatorParameterStats(animIsMovingParamaterString, AnimatorParameterType.BOOL, true);
            SetAnimatorParameterAction setRunning = new SetAnimatorParameterAction(controller.Visual.CharacterAnimator, isMoving, false);
            chase.SetUpMe(new StateAction[] { chaseTarget, setRunning });
            return chase;
        }

        protected virtual State SetUpStutter()
        {
            State stutter = new State();
            ChangeSpeedAction stopCharacter = new ChangeSpeedAction(agent, 0, false);
            AnimatorParameterStats isMoving = new AnimatorParameterStats(animIsMovingParamaterString, AnimatorParameterType.BOOL, false);
            SetAnimatorParameterAction setRunning = new SetAnimatorParameterAction(controller.Visual.CharacterAnimator, isMoving, false);

            stutter.SetUpMe(new StateAction[] { setRunning, stopCharacter });
            return stutter;
        }

        protected virtual State SetUpAttack()
        {
            State attack = new State();
            MantainSetDistanceFromPlayerAction MantainSetDistance = new MantainSetDistanceFromPlayerAction(agent, characterCurrentInfo.CharStats.BaseSpeed*0.5f, characterCurrentInfo.CharStatesStats.distanceToStartAttack-0.1f, calculusFrequency);

            AnimatorParameterStats isAttacking = new AnimatorParameterStats(animAttackString, AnimatorParameterType.TRIGGER, true);
            SetAnimatorParameterAction setAttackingAnim = new SetAnimatorParameterAction(controller.Visual.CharacterAnimator, isAttacking, 
                true, characterCurrentInfo.CharStats.AttackCountdown);
            RotateToPlayerAction rotateToTarget = new RotateToPlayerAction(agent);
            AIAttackAction attackTarget = new AIAttackAction(controller, characterCurrentInfo.CharStats.AttackCountdown, false);



            attack.SetUpMe(new StateAction[] {MantainSetDistance, setAttackingAnim, rotateToTarget, attackTarget });
            return attack;
        }

        protected State SetUpDying()
        {
            State dying = new State();

            AnimatorParameterStats isDying = new AnimatorParameterStats(animDeathString, AnimatorParameterType.BOOL, true);
            SetAnimatorParameterAction setDeathAnim = new SetAnimatorParameterAction(controller.Visual.CharacterAnimator, isDying, false);
            ChangeSpeedAction stopCharacter = new ChangeSpeedAction(agent, 0, false);

            dying.SetUpMe(new StateAction[] { stopCharacter, setDeathAnim });
            return dying;
        }

        private State SetUpBackgroundMoving()
        {
            State backgroundMoving = new State();
            AnimatorParameterStats moveAxisX = new AnimatorParameterStats(animXAxisValue, AnimatorParameterType.FLOAT, true);
            AnimatorParameterStats moveAxisZ = new AnimatorParameterStats(animZAxisValue, AnimatorParameterType.FLOAT, true);
            SetSpeedInAnimatorAction animSpeedX = new SetSpeedInAnimatorAction(controller.Visual.CharacterAnimator, agent, VectorAxis.X, moveAxisX, 0.25f, true);
            SetSpeedInAnimatorAction animSpeedZ = new SetSpeedInAnimatorAction(controller.Visual.CharacterAnimator, agent, VectorAxis.Z, moveAxisZ, 0.25f, true);

            LookDistanceForDespawnAction lookForDespawn = new LookDistanceForDespawnAction(controller.gameObject, characterCurrentInfo.CharStatesStats.distanceToFollowPlayer* 1.5f, calculusFrequency);

            backgroundMoving.SetUpMe(new StateAction[] { animSpeedX, animSpeedZ, lookForDespawn });
            return backgroundMoving;
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

        InitFSM();
    }

    protected virtual void InitFSM()
    {
        moving = SetUpBackgroundMoving();

        patrol = SetUpPatrol();
        chase = SetUpChase();
        stutter = SetUpStutter();
        attack = SetUpAttack();
        dying = SetUpDying();

        moving.SetUpMe(new Transition[] { });
        patrol.SetUpMe(new Transition[] { StartChase(patrol, stutter), });
        stutter.SetUpMe(new Transition[] { StopStutter(stutter, chase) });
        chase.SetUpMe(new Transition[] { StopChase(chase, patrol), ChaseToAttack(chase, attack) });
        attack.SetUpMe(new Transition[] { AttackBackToChase(attack, chase) });
        dying.SetUpMe(new Transition[] { });

        FSM.Init(new State[] { patrol, stutter, chase, attack, dying }, patrol, moving);
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
        controller.CombatManager.OnHealthModuleDeath += (() => FSM.SwapState(dying));

    }

    protected void CharacterStatsConfiguration()
    {
        // To change respecting random multipliers
        characterCurrentInfo.CharStats = characterStartingInfo.CharInfo.CharStats;
        characterCurrentInfo.CharStatesStats = characterStartingInfo.CharInfo.CharStatesStats;
        characterCurrentInfo.CharNarrativeStats = characterStartingInfo.CharInfo.CharNarrativeStats;

        controller.CharacterInfo = characterCurrentInfo;

        CalculateDamage();

        // Unpossessable EnemyChar behavior
        ObjectByProbability<bool> unpossessableProb = new ObjectByProbability<bool>();
        unpossessableProb.Max = characterStartingInfo.CharStats.UnpossessableProbability;

        controller.UnPossessable = unpossessableProb.IsInRange(UnityEngine.Random.Range(0f, 1f));
        if (controller.UnPossessable && !controller.IsPossessed)
        {
            Debug.Log("Spawn unposessable enemyChar");

            // Unpossessable Enemy differences with normal enemy
            SkinnedMeshRenderer mesh = controller.Visual.GetComponentInChildren<SkinnedMeshRenderer>();
            List<Material> unpossessableMaterials = mesh.materials.ToList();
            unpossessableMaterials.Add(unpossesableMaterial);
            mesh.SetMaterials(unpossessableMaterials);

        }
        else
        {

            SkinnedMeshRenderer mesh = controller.Visual.GetComponentInChildren<SkinnedMeshRenderer>();
            List<Material> unpossessableMaterials = mesh.materials.ToList();
            if (unpossessableMaterials.Count > 1) 
            {
                //Trovare soluzione!!!!!!!
                try
                {
                    unpossessableMaterials.RemoveAt(1);
                    mesh.SetMaterials(unpossessableMaterials);
                }
                catch 
                {
                }
                /////////////
            }
        }
    }

    protected void CalculateDamage()
    {
        int playerLevel = PlayerState.Get().LevelController.GetCurrentLevel();
        characterCurrentInfo.CharStats.Damage *= UnityEngine.Random.Range(CharacterInfo.CharStats.MinDamageMultiplier, CharacterInfo.CharStats.MaxDamageMultiplier) * playerLevel;
    }
    #endregion

    #region Enemy
    public abstract void CastAbility();

    public virtual void InternalPossess()
    {
        FSM.enabled = false;
        agent.enabled = false;


        GlobalEventSystem.CastEvent(
            EventName.PossessionExecuted, 
            EventArgsFactory.PossessionExecutedFactory(characterCurrentInfo)
            );
    }

    public virtual void InternalUnPossess()
    {
        FSM.enabled = true;
        agent.enabled = true;
    }
    #endregion
}
