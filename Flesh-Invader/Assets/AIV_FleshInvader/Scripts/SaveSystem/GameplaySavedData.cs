using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Do things


        savedFileVersion = CurrentFileVersion;
    }

    public virtual void OnCreation()
    {
        
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

    public void OnLoadedFromDisk()
    {
        throw new System.NotImplementedException();
    }

    public void OnPostSave()
    {
        throw new System.NotImplementedException();
    }

    public void OnPreSave()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
