using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatisticsTemplate",
    menuName = "EnemyTemplate/EnemyStatisticsTemplate", order = 1)]
public class EnemyStatisticsTemplate : ScriptableObject
{

    [SerializeField] private EnemyInfo charInfo;

    #region StructProperties
    public EnemyStatistics CharStats { get => charInfo.CharStats; }
    public EnemyStateStats CharStatesStats { get => charInfo.CharStatesStats; }
    public EnemyInfo CharInfo { get => charInfo; set => charInfo = value; }
    #endregion

    #region CharStatsProperties
    public EnemyType EnemyType { get { return CharStats.EnemyType; } }
    public float Health { get { return CharStats.Health; } }
    public float Damage { get { return CharStats.Damage; } }
    public float Speed { get { return CharStats.BaseSpeed; } }
    public float ChaseSpeed { get { return CharStats.ChaseSpeed; } }
    public float XP { get { return CharStats.Xp; } }
    #endregion

    #region StateParamsProperties
    #region Distances
    public float DistanceToFollowPlayer { get { return CharStatesStats.distanceToFollowPlayer; } }
    public float DistanceToStopFollowPlayer { get { return CharStatesStats.distanceToStopFollowPlayer; } }
    public float DistanceToStartAttack { get { return CharStatesStats.distanceToStartAttack; } }
    public float DistanceToStopAttack { get { return CharStatesStats.distanceToStopAttack; } }
    #endregion

    #region Patrol
    public float PatrolAcceptableRadius { get { return CharStatesStats.patrolAcceptableRadius; } }
    public float PatrolPointsGenerationRadius { get { return CharStatesStats.patrolPointsGenerationRadius; } }
    public int   PatrolPointNumber { get { return CharStatesStats.patrolPointNumber; } }
    #endregion

    #region Chase
    public float ChaseSpeedMultiplier { get { return CharStatesStats.chaseSpeedMultiplier; } }
    #endregion

    #region Stutter
    public float StutterTime { get { return CharStatesStats.stutterTime; } }
    public Material TestStutterMaterial { get { return CharStatesStats.testStutterMaterial; } }
    #endregion

    #endregion

}
