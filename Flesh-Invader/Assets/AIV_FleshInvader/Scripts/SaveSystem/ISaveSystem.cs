using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveSystem
{

    void Initialize();

    void SaveSettingsData();
    void CreateSettingsData();
    void DeleteSettingsData();
    bool SettingDataExists();
    void CreateGameData(int slotIndex);
    void LoadAllSlotData();
    void SaveActiveGameData();
    void DeleteGameData(int slotIndex);
    void SelectGameData(int slotIndex);
    bool GameDataExists(int slotIndex);
}
