using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelStruct
{
    private int currentXP;
    private int nextLevelXp;

    private int currentLevel;

    public int CurrentXP { get { return currentXP; } set { currentXP = value; } }
    public int NextLevelXp {  get { return nextLevelXp; } set { nextLevelXp = value; } }
    public int CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
}
