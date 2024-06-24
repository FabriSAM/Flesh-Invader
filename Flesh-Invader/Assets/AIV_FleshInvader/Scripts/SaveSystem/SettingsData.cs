using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SettingsData : ISaveableDataClass
{

    private string resolution;


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
        savedFileVersion = CurrentFileVersion;
    }

    public void OnDataDeselected()
    {
        // TO CHOSE IF AVAILABLE
    }

    public void OnDataSelected()
    {
        // TO ADAPT THE GAME TO SELECTED SETTINGS
    }

    public void OnDelete()
    {
        // AT SAVE FILE DELETATION
    }

    public void OnLoadedFromDisk()
    {
        // AT SAVE FILE LOAD
    }
    public void OnPreSave()
    {
        // BEHAVIORS BEFORE SAVE
    }

    public void OnPostSave()
    {
        // BEHAVIORS AFTER SAVE
    }

}
