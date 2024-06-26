using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void Awake()
    {
        //SaveSystem.CreateGameData(0);
        SaveSystem.LoadAllSlotData();
    }

    private void OnTriggerEnter(Collider other)
    {
        SaveSystem.ActiveGameData.PlayerSavedData.UpdateLastCheckpointPosition(this.transform.position);
        SaveSystem.ActiveGameData.PlayerSavedData.UpdatePlayerStats(PlayerState.Get().InformationController.GetStats());


        SaveSystem.SaveActiveGameData();
    }

}
