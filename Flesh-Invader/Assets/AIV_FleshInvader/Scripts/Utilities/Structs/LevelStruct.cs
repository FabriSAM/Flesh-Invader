using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelStruct
{    
    private float currentXP;
    [SerializeField]
    private float nextLevelXp;
    [SerializeField]
    private int currentLevel;

    public float CurrentXP { get { return currentXP; } set { currentXP = value; Debug.Log("CurrentXP: " + currentXP); } }
    public float NextLevelXp {  get { return nextLevelXp; } set { nextLevelXp = value; Debug.Log("CurrentXP: " + currentXP); } }
    public int CurrentLevel { get { return currentLevel; } set { currentLevel = value; Debug.Log("CurrentXP: " + currentXP); } }
}
