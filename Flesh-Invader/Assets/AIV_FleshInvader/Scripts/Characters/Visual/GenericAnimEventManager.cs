using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAnimEventManager : MonoBehaviour
{
    [SerializeField]
    private Controller controller;

    private void EndDeathAnimation()
    {
        if (controller == null) return;
        controller.OnCharacterDeathEnd?.Invoke();
    }
}
