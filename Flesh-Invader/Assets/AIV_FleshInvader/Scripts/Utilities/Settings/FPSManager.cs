using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    [SerializeField]
    private bool defalutVSync;
    [SerializeField]
    private int startTargetFrameRate;

    private void Awake()
    {
        GraphicSettingsUtility.SetVSync(defalutVSync);
        GraphicSettingsUtility.SetFrameRate(startTargetFrameRate);
    }
}
