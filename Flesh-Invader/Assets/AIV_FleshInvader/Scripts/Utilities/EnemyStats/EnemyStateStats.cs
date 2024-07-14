using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public struct EnemyStateStats
{
    [Header("StateParameters")]
    [SerializeField] public float distanceToFollowPlayer;
    [SerializeField] public float distanceToStopFollowPlayer;
    [SerializeField] public float distanceToStartCombat;
    [SerializeField] public float distanceToStopCombat;
    [SerializeField] public float distanceToStartAttack;
    [SerializeField] public float distanceToStopAttack;

    [Category("Patrol")]
    [SerializeField] public float patrolAcceptableRadius;
    [SerializeField] public float patrolPointsGenerationRadius;
    [SerializeField] public int patrolPointNumber;

    [Category("Chase")]
    [Tooltip("The value patrol speed is multiplied to obtain chase speed")]
    [SerializeField] public float chaseSpeedMultiplier;

    [Header("Stutter")]
    [SerializeField] public float stutterTime;

}
