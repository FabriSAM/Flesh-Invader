using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSavedData : GameplaySavedData
{
    #region AttributeToSave
    private Statistics currentPlayerStats;

    // TO move into a LevelSavedData class?
    private List<int> unlockedCollectibleIDs;
    private Vector3 lastCheckpoint;

    #endregion

    #region HandleData
    public Vector3 SavedLastCheckpoint {  get { return lastCheckpoint; } }

    public Statistics savedStatistics { get { return currentPlayerStats; } }

    public bool CollectibleUnlocked(int collectibleID)
    {
        return unlockedCollectibleIDs != null && unlockedCollectibleIDs.Contains(collectibleID);
    }

    public void UnlockCollectible(int collectibleID)
    {
        if (unlockedCollectibleIDs.Contains(collectibleID)) return;
        unlockedCollectibleIDs.Add(collectibleID);
    }
    #endregion


    #region SaveableDataClass

    public override void OnCreation()
    {
        currentPlayerStats = PlayerState.Get().InformationController.GetStats();
        unlockedCollectibleIDs = new List<int>();
        lastCheckpoint = new Vector3();
    }

    #endregion
}
