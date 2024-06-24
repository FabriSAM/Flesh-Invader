using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystemConfiguration
{

    private const string settingPath = "/Settings/";
    private const string gameDataPath = "/Data/";

    private const string settingsFileName = "Settings.file";
    private const string gameDataNameAffix = "GameData_";
    private const string gameDataNameSuffix = ".file";

    private const int gameDataSlotNumber = 3;

    public static string RootsPath
    {
        get { return Application.persistentDataPath; }
    }

    public static string GameDataFolderPath
    {
        get { return RootsPath + gameDataPath; }
    }

    public static string SettingsFolderPath
    {
        get { return RootsPath + settingPath; }
    }

    public static int GameDataSlotNumber
    {
        get { return gameDataSlotNumber; }
    }

    public static string GetGameDataPath(int slot)
    {
        return GameDataFolderPath + gameDataNameAffix + slot.ToString() + gameDataNameSuffix;
    }
}
