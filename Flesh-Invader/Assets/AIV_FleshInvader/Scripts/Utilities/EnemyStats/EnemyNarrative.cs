using System;
using UnityEngine;

[Serializable]
public struct EnemyNarrative
{
    [SerializeField] public Sprite icon;
    [SerializeField] public string enemyTypeDescription;
    [SerializeField] public string baseAttackDescription;
    [SerializeField] public string passiveSkillDescription;
}
