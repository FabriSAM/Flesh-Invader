using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyInfo
{
    [SerializeField] public EnemyStatistics CharStats;
    [SerializeField] public EnemyStateStats CharStatesStats;
    [SerializeField] public EnemyNarrative  CharNarrativeStats;

}
