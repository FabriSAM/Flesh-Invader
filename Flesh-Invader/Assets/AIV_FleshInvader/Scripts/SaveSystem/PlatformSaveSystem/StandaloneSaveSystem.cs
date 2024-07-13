using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StandAloneSaveSystem : ISaveSystem
{

    private SettingsData settingsData;
    private GameSavedData[] allDatas;

    public GameSavedData ActiveGameData
    {
        get;
        private set;
    }

    private int currentSlotIndex;

    public void Initialize()
    {
        allDatas = new GameSavedData[SaveSystemConfiguration.GameDataSlotNumber];
        if (!Directory.Exists(SaveSystemConfiguration.SettingsFolderPath))
        {
            Directory.CreateDirectory(SaveSystemConfiguration.SettingsFolderPath);
        }
        if (!Directory.Exists(SaveSystemConfiguration.GameDataFolderPath))
        {
            Directory.CreateDirectory(SaveSystemConfiguration.GameDataFolderPath);
        }

        // If settingsData file doesn't exists i create it
        if (!SettingDataExists())
        {
            CreateSettingsData();
        }
        // If it exists i load it from file
        else
        {
            settingsData = LoadISaveableData<SettingsData>(SaveSystemConfiguration.SettingsFilePath);
        }
    }
   
    #region GameData

    public void CreateGameData(int slotIndex)
    {
        allDatas[slotIndex] = CreateISaveableData<GameSavedData>();
        SaveISaveableData<GameSavedData>(SaveSystemConfiguration.GetGameDataPath(slotIndex), allDatas[slotIndex]);
        ActiveGameData = allDatas[slotIndex];
    }

    public void DeleteGameData(int slotIndex)
    {
        LoadSlotData(slotIndex);
        DeleteISaveableData<GameSavedData>(SaveSystemConfiguration.GetGameDataPath(slotIndex), allDatas[slotIndex]);
    }

    public bool GameDataExists(int slotIndex)
    {
        return File.Exists(SaveSystemConfiguration.GetGameDataPath(slotIndex));
    }

    #region LoadSlots
    public void LoadSlotData(int slotIndex)
    {
        string path = SaveSystemConfiguration.GetGameDataPath(slotIndex);
        if (GameDataExists(slotIndex))
        {
            allDatas[slotIndex] = LoadISaveableData<GameSavedData>(path);
        }
        else
        {
            allDatas[slotIndex] = null;
        }
        SelectGameData(slotIndex);
    }

    public void LoadAllSlotData()
    {
        for (int i = 0; i < allDatas.Length; i++)
        {
            string path = SaveSystemConfiguration.GetGameDataPath(i);
            if (GameDataExists(i))
            {
                allDatas[i] = LoadISaveableData<GameSavedData>(path);
            }
            else
            {
                allDatas[i] = null;
            }
        }
        ActiveGameData = allDatas[currentSlotIndex];
    }

    #endregion

    public void SaveActiveGameData()
    {
        Debug.Log("Save happened. Active Game Data: \n Last Position - "+ ActiveGameData.PlayerSavedData.SavedLastCheckpoint);
        SaveISaveableData<GameSavedData>(SaveSystemConfiguration.GetGameDataPath(currentSlotIndex), ActiveGameData);
    }

    public void SelectGameData(int slotIndex)
    {
        if(ActiveGameData != null)
        {
            ActiveGameData.OnDataDeselected();
        }
        if(slotIndex == -1)
        {
            ActiveGameData = null;
            currentSlotIndex = -1;
            return;
        }
        ActiveGameData = allDatas[slotIndex];
        ActiveGameData.OnDataDeselected();
        currentSlotIndex = slotIndex;
    }
    #endregion 

    #region SettingsData
    public void CreateSettingsData()
    {
        settingsData = CreateISaveableData<SettingsData>();
        SaveSettingsData();
    }

    public void SaveSettingsData()
    {
        SaveISaveableData<SettingsData>(SaveSystemConfiguration.SettingsFilePath, settingsData);
    }

    public bool SettingDataExists()
    {
        return File.Exists(SaveSystemConfiguration.SettingsFilePath);
    }
    public void DeleteSettingsData()
    {
        DeleteISaveableData<SettingsData>(SaveSystemConfiguration.SettingsFilePath, settingsData);
    }
    #endregion

    #region WrapperSaveable
    private static T CreateISaveableData<T>() where T : ISaveableDataClass, new ()
    {
        T data = new T();
        data.OnCreation();
        return data;
    }

    private void SaveISaveableData<T> (string path, ISaveableDataClass dataToSave) where T : ISaveableDataClass, new()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.OpenOrCreate);
        file.Seek(0, SeekOrigin.Begin);

        dataToSave.OnPreSave();
        bf.Serialize(file, dataToSave.InstanceToSave);
        dataToSave.OnPostSave();
        file.Close();
    }

    private T LoadISaveableData<T> (string path) where T : ISaveableDataClass
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.OpenOrCreate);
        file.Seek(0, SeekOrigin.Begin);
        T data = (T)bf.Deserialize(file);
        file.Close();
        if (!data.CheckVersion()){
            data.HandleVersionChanged();
        }
        data.OnLoadedFromDisk();
        return data;
    }

    private static void DeleteISaveableData<T>(string path, ISaveableDataClass data) where T: ISaveableDataClass
    {
        data.OnDelete();
        File.Delete(path);
    }

    public void SaveGameParams(Vector3 spawnPosition)
    {
        #region Save: PlayerGeneralParameters
        SaveSystem.ActiveGameData.PlayerSavedData.UpdateLastCheckpointPosition(spawnPosition);
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
    }

    #endregion


}
