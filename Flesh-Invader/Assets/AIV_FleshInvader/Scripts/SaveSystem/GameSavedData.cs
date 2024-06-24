using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSavedData : ISaveableDataClass
{

    private List<ISaveableDataClass> saveableDatas;

    #region DataToSave
    private PlayerSavedData playerSavedDatas;
    #endregion

    #region SaveableDataClass
    public float CurrentFileVersion
    {
        get { return 1.0f; }
    }

    private float savedFileVersion;
    public float SavedFileVersion
    {
        get { return savedFileVersion; }
    }

    public object InstanceToSave
    {
        get { return this; }
    }

    public bool CheckVersion()
    {
        return CurrentFileVersion == SavedFileVersion;
    }

    public void HandleVersionChanged()
    {
        // Manage version change
        savedFileVersion = CurrentFileVersion;
    }

    public void OnCreation()
    {
        saveableDatas = new List<ISaveableDataClass>();

        // Index 0: Player's Datas
        playerSavedDatas = new PlayerSavedData();
        saveableDatas.Add(playerSavedDatas);

        foreach(ISaveableDataClass data in saveableDatas)
        {
            playerSavedDatas.OnCreation();
        }
    }

    public void OnDataDeselected()
    {
        foreach (ISaveableDataClass data in saveableDatas)
        {
            playerSavedDatas.OnDataDeselected();
        }
    }

    public void OnDataSelected()
    {
        foreach (ISaveableDataClass data in saveableDatas)
        {
            playerSavedDatas.OnDataSelected();
        }
    }

    public void OnDelete()
    {
        foreach (ISaveableDataClass data in saveableDatas)
        {
            playerSavedDatas.OnDelete();
        }
    }

    public void OnLoadedFromDisk()
    {
        foreach (ISaveableDataClass data in saveableDatas)
        {
            playerSavedDatas.OnLoadedFromDisk();
        }
    }

    public void OnPostSave()
    {
        foreach (ISaveableDataClass data in saveableDatas)
        {
            playerSavedDatas.OnPostSave();
        }
    }

    public void OnPreSave()
    {
        foreach (ISaveableDataClass data in saveableDatas)
        {
            playerSavedDatas.OnPreSave();
        }
    }
    #endregion

    #region WrapData
    public PlayerSavedData PlayerSavedData
    {
        get { return (PlayerSavedData)saveableDatas[0]; }
    }
    #endregion
}
