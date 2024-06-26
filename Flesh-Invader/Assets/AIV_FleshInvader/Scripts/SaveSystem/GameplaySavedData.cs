using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class GameplaySavedData : ISaveableDataClass
{

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
        // Do stuff


        savedFileVersion = CurrentFileVersion;
    }

    public virtual void OnCreation()
    {
        //Do stuff
    }

    public void OnDataDeselected()
    {
        // Do things
    }

    public void OnDataSelected()
    {
        // Do things to update player status
    }

    public void OnDelete()
    {
        // Before deletation
    }

    public virtual void OnLoadedFromDisk()
    {
        // Do stuff
    }

    public void OnPostSave()
    {
        // Do stuff
    }

    public void OnPreSave()
    {
        // Do stuff
    }
    #endregion
}
