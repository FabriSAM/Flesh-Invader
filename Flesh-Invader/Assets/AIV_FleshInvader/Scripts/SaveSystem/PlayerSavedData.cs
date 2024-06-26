using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    // TO move into a LevelSavedData class?
    private List<int> unlockedCollectibleIDs;
    private SerializableVector3 lastCheckpoint;

    #endregion

    #region HandleData
    public Vector3 SavedLastCheckpoint {  get { return lastCheckpoint.returnVector(); } }

    public Statistics savedStatistics { get { return currentPlayerStats; } }

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

    #endregion

    #region SaveableDataClass

    public override void OnCreation()
    {
        currentPlayerStats = PlayerState.Get().InformationController.GetStats();
        unlockedCollectibleIDs = new List<int>();
        lastCheckpoint = new SerializableVector3(new Vector3());
    }

    public override void OnLoadedFromDisk()
    {
        // Player moves to spawn position
        PlayerState.Get().CurrentPlayer.transform.position = lastCheckpoint.returnVector();

        //PlayerState.Get().InformationController.stat = currentPlayerStats;
    }
    #endregion
}
