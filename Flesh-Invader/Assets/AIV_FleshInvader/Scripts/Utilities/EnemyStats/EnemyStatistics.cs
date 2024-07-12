using System;
using UnityEngine;

[Serializable]
public struct EnemyStatistics
{
    [SerializeField]
    public EnemyType EnemyType;
    [Header("Stats")]
    [SerializeField]
    public float Health;
    [SerializeField]
    public float MinHealthMultiplier;
    [SerializeField]
    public float MaxHealthMultiplier;
    [SerializeField]
    public float Damage;
    [SerializeField]
    public float MinDamageMultiplier;
    [SerializeField]
    public float MaxDamageMultiplier;
    [SerializeField]
    public float BaseSpeed;
    [SerializeField]
    public float ChaseSpeed;
    [SerializeField]
    public float Xp;

    [SerializeField]
    public bool CanLockpick;
    [Tooltip("Probability of spawn this enemyChar with the unposessable characteristic, calculated from 0 (Always Possessable) to value, with a max of 1 (Always Unpossessable)")]
    [SerializeField]
    [Range(0,1)]
    public float UnpossessableProbability;
    [SerializeField]
    public float AttackCountdown;
}
