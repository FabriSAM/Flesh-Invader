using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Vector3 spawnPositionOffset;
    private float offsetSphereRadius = 3;

    private void OnTriggerEnter(Collider other)
    {
        SaveSystem.ActiveGameData.PlayerSavedData.UpdateLastCheckpointPosition(transform.position + spawnPositionOffset);
        SaveSystem.ActiveGameData.PlayerSavedData.UpdatePlayerStats(PlayerState.Get().InformationController.GetStats());

        GameObject player = PlayerState.Get().CurrentPlayer;
        EnemyChar playerChar = player.GetComponentInChildren<EnemyChar>();
        EnemyInfo playerInfo = playerChar.CharacterInfo;

        SaveSystem.ActiveGameData.PlayerSavedData.UpdatePlayerCharInfo(playerInfo);

        SaveSystem.SaveActiveGameData();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + spawnPositionOffset, offsetSphereRadius);
    }
}
