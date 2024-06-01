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
    public float Damage;
    [SerializeField]
    public float Speed;
    [SerializeField]
    public float Xp;

    [SerializeField]
    public bool CanLockpick;
}
