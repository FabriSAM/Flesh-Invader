using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GraphicSettingsUtility
{
    public static void SetVSync(bool vSync)
    {
        if (vSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public static void SetFrameRate(int fps)
    {
        Application.targetFrameRate = fps;
    }
}
