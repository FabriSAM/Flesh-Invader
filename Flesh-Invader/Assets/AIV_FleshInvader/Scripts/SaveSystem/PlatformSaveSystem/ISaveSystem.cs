using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveSystem
{
    GameSavedData ActiveGameData 
    {
        get;
    }

    void Initialize();

    void SaveSettingsData();
    void CreateSettingsData();
    void DeleteSettingsData();
    void CreateGameData(int slotIndex);

    void LoadSlotData(int slotIndex);
    void LoadAllSlotData();
    void SaveActiveGameData();
    void SaveGameParams(Vector3 spawnPosition);

    void DeleteGameData(int slotIndex);
    void SelectGameData(int slotIndex);

    bool SettingDataExists();
    bool GameDataExists(int slotIndex);
}
