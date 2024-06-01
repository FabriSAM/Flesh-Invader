using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatisticsTemplate",
    menuName = "EnemyTemplate/EnemyStatisticsTemplate", order = 1)]
public class EnemyStatisticsTemplate : ScriptableObject
{
    [SerializeField]
    private EnemyType enemyType;
    [Header("Stats")]
    [SerializeField]
    private float health;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float xp;

    [Header("Narrative")]
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private string passiveAbilityDescription;
    [SerializeField]
    private string baseAbilityDescription;

    [Header("StateParameters")]
    [SerializeField] private float distanceToFollowPlayer;
    [SerializeField] private float distanceToStopFollowPlayer;
    [SerializeField] private float distanceToStartAttack;
    [SerializeField] private float distanceToStopAttack;

    [Category("Patrol")]
    [SerializeField] private float patrolAcceptableRadius;
    [SerializeField] private float patrolPointsGenerationRadius;
    [SerializeField] private int patrolPointNumber;

    [Category("Chase")]
    [Tooltip("The value patrol speed is multiplied to obtain chase speed")]
    [SerializeField] protected float chaseSpeedMultiplier;

    [Header("Stutter")]
    [SerializeField] private float stutterTime;
    [SerializeField] private Material testStutterMaterial;


    public EnemyType EnemyType { get { return enemyType; } }
    public Sprite Icon { get { return icon; } }
    public string PassiveAbilityDescription { get { return passiveAbilityDescription; } }
    public string BaseAbilityDescription { get { return baseAbilityDescription; } }
    public float Health { get { return health; } }
    public float Damage { get { return damage; } }
    public float Speed { get { return speed; } }
    public float XP { get { return xp; } }

    #region StateParamsProperties
    #region Distances
    public float DistanceToFollowPlayer { get { return distanceToFollowPlayer; } }
    public float DistanceToStopFollowPlayer { get { return distanceToStopFollowPlayer; } }
    public float DistanceToStartAttack { get { return distanceToStartAttack; } }
    public float DistanceToStopAttack { get { return distanceToStopAttack; } }
    #endregion

    #region Patrol
    public float PatrolAcceptableRadius { get { return patrolAcceptableRadius; } }
    public float PatrolPointsGenerationRadius { get { return patrolPointsGenerationRadius; } }
    public int   PatrolPointNumber { get { return patrolPointNumber; } }
    #endregion

    #region Chase
    public float ChaseSpeedMultiplier { get { return chaseSpeedMultiplier; } }
    #endregion

    #region Stutter
    public float StutterTime { get { return stutterTime; } }
    public Material TestStutterMaterial { get { return testStutterMaterial; } }
    #endregion

    #endregion

    private void OnDestroy()
    {
       
    }
}
