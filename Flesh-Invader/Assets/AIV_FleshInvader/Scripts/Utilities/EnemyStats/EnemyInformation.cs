using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyInformation
{
    [SerializeField]
    EnemyType enemyType;
    [SerializeField]
    EnemyNarrativeTemplate templates;

    private EnemyStatisticsTemplate enemyStats;

    public EnemyStatisticsTemplate Stats { get { return enemyStats; } }

    public EnemyInformation(EnemyType type)
    {
        enemyType = type;
        for (int i = 0; i< (int)type; i++)
        {
            if(templates.EnemyStatisticsTemplates[i].EnemyType == enemyType)
            {
                enemyStats = templates.EnemyStatisticsTemplates[i];
            }
        }
    }
}
