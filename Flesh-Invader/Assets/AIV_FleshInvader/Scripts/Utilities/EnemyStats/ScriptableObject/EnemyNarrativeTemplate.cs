using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDB",
    menuName = "EnemyTemplate/EnemyDB", order = 2)]
public class EnemyNarrativeTemplate : ScriptableObject
{
    [SerializeField]
    EnemyStatisticsTemplate[] enemyStatisticsTemplates;

    public EnemyStatisticsTemplate[] EnemyStatisticsTemplates { get { return enemyStatisticsTemplates; } }
}
