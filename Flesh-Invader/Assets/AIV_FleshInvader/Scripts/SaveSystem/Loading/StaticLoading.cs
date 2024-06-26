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
            
            CharacterSpawner.GetInstance().LoadPlayerCharacter(SaveSystem.ActiveGameData.PlayerSavedData.PlayerCharInfo);
            
        }
    }
}
