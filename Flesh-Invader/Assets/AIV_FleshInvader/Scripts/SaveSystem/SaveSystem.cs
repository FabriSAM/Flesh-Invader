using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem 
{
    public static GameSavedData ActiveGameData
    {
        get { return platformBasedSaveSystem.ActiveGameData; }
    }
    
    static ISaveSystem platformBasedSaveSystem;

    static SaveSystem()
    {
        #if UNITY_STANDALONE
                platformBasedSaveSystem = new StandAloneSaveSystem();
        #elif UNITY_XBOX

        #elif UNITY_PS5

        #endif

        platformBasedSaveSystem.Initialize();
    }

    #region SettingsData
    public static void SaveSettingData()
    {
        platformBasedSaveSystem.SaveSettingsData();
    }

    public static void CreateSettingsData()
    {
        platformBasedSaveSystem.CreateSettingsData();
    }

    public static void DeleteSettingsData()
    {
        platformBasedSaveSystem.DeleteSettingsData();
    }

    public static bool SettingsDataExists()
    {
        return platformBasedSaveSystem.SettingDataExists();
    }

    #endregion

    #region GameData
    public static void CreateGameData(int slotIndex)
    {
        platformBasedSaveSystem.CreateGameData(slotIndex);
    }
    
    public static void LoadSlotData(int slotIndex)
    {
        platformBasedSaveSystem.LoadSlotData(slotIndex);
    }

    public static void LoadAllSlotData()
    {
        platformBasedSaveSystem.LoadAllSlotData();
    }

    public static void SaveActiveGameData()
    {
        platformBasedSaveSystem.SaveActiveGameData();
    }
    
    public static void DeleteGameData(int slotIndex)
    {
        platformBasedSaveSystem.DeleteGameData(slotIndex);
    }

    public static void SelectGameData(int slotIndex)
    {
        platformBasedSaveSystem.SelectGameData(slotIndex);
    }

    public static bool GameDataExists(int slotIndex)
    {
        return platformBasedSaveSystem.GameDataExists(slotIndex);
    }

    #endregion


}
