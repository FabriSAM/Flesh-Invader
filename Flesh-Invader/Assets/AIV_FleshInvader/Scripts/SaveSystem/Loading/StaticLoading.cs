using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StaticLoading
{
    
    public static bool LoadSaveGame {get; set;}
    static StaticLoading()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        if (LoadSaveGame)
        {
            GlobalEventSystem.CastEvent(EventName.LoadGameEnded, EventArgsFactory.LoadGameEndedFactory());
            
            // To refactor 
            CharacterSpawner.GetInstance().LoadPlayerCharacter(
                SaveSystem.ActiveGameData.PlayerSavedData.PlayerMaxHealth,
                SaveSystem.ActiveGameData.PlayerSavedData.PlayerHealth,
                SaveSystem.ActiveGameData.PlayerSavedData.SavedLastCheckpoint, 
                SaveSystem.ActiveGameData.PlayerSavedData.savedStatistics, 
                SaveSystem.ActiveGameData.PlayerSavedData.LevelStruct,
                SaveSystem.ActiveGameData.PlayerSavedData.PlayerCharInfo 
                );;
            
        }
    }
}
