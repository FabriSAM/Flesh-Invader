using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Vector3 spawnPositionOffset;
    private float offsetSphereRadius = 3;

    private void OnTriggerEnter(Collider other)
    {
        #region Save: PlayerGeneralParameters
        SaveSystem.ActiveGameData.PlayerSavedData.UpdateLastCheckpointPosition(transform.position + spawnPositionOffset);
        SaveSystem.ActiveGameData.PlayerSavedData.UpdatePlayerStats(PlayerState.Get().InformationController.GetStats());
        SaveSystem.ActiveGameData.PlayerSavedData.UpdatePlayerLevel(PlayerState.Get().LevelController.GetLevelStruct());
        #endregion

        #region Save: EnemyCharInfo
        GameObject player = PlayerState.Get().CurrentPlayer;
        EnemyChar playerChar = player.GetComponentInChildren<EnemyChar>();
        EnemyInfo playerInfo = playerChar.CharacterInfo;
        SaveSystem.ActiveGameData.PlayerSavedData.UpdatePlayerCharInfo(playerInfo);
        #endregion

        #region Save: PlayerHealth
        SaveSystem.ActiveGameData.PlayerSavedData.UpdatePlayerHealth(PlayerState.Get().HealthController.GetCurrentHealth());
        SaveSystem.ActiveGameData.PlayerSavedData.UpdatePlayerMaxHealth(PlayerState.Get().HealthController.GetMaxHealth());
        #endregion

        SaveSystem.SaveActiveGameData();
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + spawnPositionOffset, offsetSphereRadius);
    }
}
