using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSUtility : MonoBehaviour
{
    [SerializeField]
    private bool startEnabledVSync;
    [SerializeField]
    private int startNewFPS;

    private void Awake()
    {
       SetVSync(startEnabledVSync);
       SetFrameRate(startNewFPS); 
    }

    public void SetVSync(bool vSync)
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

    public void SetFrameRate(int fps)
    {
        Application.targetFrameRate = fps;
    }
}
