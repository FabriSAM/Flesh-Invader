using System;
using UnityEngine;

[Serializable]
public struct EnemyNarrative
{
    [SerializeField] public string iconTextureName;
    [SerializeField] public string colorName;
    [SerializeField] public string enemyTypeDescription;
    [SerializeField] public string baseAttackDescription;
    [SerializeField] public string passiveSkillDescription;
}
