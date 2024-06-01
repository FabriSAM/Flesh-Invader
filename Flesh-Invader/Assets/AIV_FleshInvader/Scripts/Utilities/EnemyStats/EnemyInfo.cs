using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyInfo
{
    [SerializeField] private EnemyStatistics charStats;
    [SerializeField] private EnemyStateStats charStatesStats;


    public EnemyStatistics CharStats { get => charStats; set => charStats = value; }
    public EnemyStateStats CharStatesStats { get => charStatesStats; set => charStatesStats = value; }
}
