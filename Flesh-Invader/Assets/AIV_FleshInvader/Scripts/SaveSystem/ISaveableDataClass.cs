using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveableDataClass
{

    #region Versioning
    float CurrentFileVersion
    {
        get;
    }
 
    float SavedFileVersion
    {
        get;
    }

    object InstanceToSave
    {
        get;
    }

    bool CheckVersion();
    void HandleVersionChanged();
    #endregion

    #region SaveLoadBehavior
    void OnCreation();  // New Save File creation, used for initializing

    void OnLoadedFromDisk();

    void OnPreSave();

    void OnPostSave();

    void OnDelete();

    void OnDataSelected();

    void OnDataDeselected();
    #endregion

}
