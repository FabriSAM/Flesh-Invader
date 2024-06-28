using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SerializableVector3
{
    float x, y, z;

    public SerializableVector3(Vector3 originalVector)
    {
        x = originalVector.x;
        y = originalVector.y;
        z = originalVector.z;
    }

    public Vector3 returnVector()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
public class PlayerSavedData : GameplaySavedData
{
    #region AttributeToSave
    private Statistics currentPlayerStats;

    private EnemyInfo playerCharInfo;
    private float playerMaxHealth;
    private float playerHealth;
    private LevelStruct levelStruct;


    // TO move into a LevelSavedData class?
    private List<int> unlockedCollectibleIDs;
    private SerializableVector3 lastCheckpoint;

    #endregion

    #region HandleData
    public Vector3 SavedLastCheckpoint {  get { return lastCheckpoint.returnVector(); } }

    public Statistics savedStatistics { get { return currentPlayerStats; } }

    public EnemyInfo PlayerCharInfo { get {  return playerCharInfo; } }

    public LevelStruct LevelStruct { get => levelStruct; set => levelStruct = value; }
    public float PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public float PlayerMaxHealth { get => playerMaxHealth; set => playerMaxHealth = value; }

    public bool IsCollectibleUnlocked(int collectibleID)
    {
        return unlockedCollectibleIDs != null && unlockedCollectibleIDs.Contains(collectibleID);
    }

    public void UnlockCollectible(int collectibleID)
    {
        if (unlockedCollectibleIDs.Contains(collectibleID)) return;
        unlockedCollectibleIDs.Add(collectibleID);
    }

    public void UpdateLastCheckpointPosition(Vector3 newCheckpoint)
    {
        lastCheckpoint = new SerializableVector3(newCheckpoint);
    }

    public void UpdatePlayerStats(Statistics stats)
    {
        currentPlayerStats = stats;
    }

    public void UpdatePlayerCharInfo(EnemyInfo playerInfo)
    {
        playerCharInfo = playerInfo;
    }

    public void UpdatePlayerLevel(LevelStruct levelToSave)
    {
        levelStruct = levelToSave;
    }

    public void UpdatePlayerHealth(float healthToSave)
    {
        playerHealth = healthToSave;
    }

    public void UpdatePlayerMaxHealth(float maxHealthToSave)
    {
        playerMaxHealth = maxHealthToSave;
    }
    #endregion

    #region SaveableDataClass

    public override void OnCreation()
    {
        currentPlayerStats = new Statistics();
        unlockedCollectibleIDs = new List<int>();
        lastCheckpoint = new SerializableVector3(new Vector3());
        levelStruct = new LevelStruct();
    }
    #endregion
}
