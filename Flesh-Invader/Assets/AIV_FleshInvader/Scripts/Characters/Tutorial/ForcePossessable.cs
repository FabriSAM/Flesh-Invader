using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePossessable : MonoBehaviour
{
    [SerializeField]
    private Controller controller;
    [SerializeField]
    private Material unpossesableMaterial;
    // Start is called before the first frame update
    void Start()
    {
        controller.UnPossessable = false;
        controller.Overlay.RemoveOverlay(unpossesableMaterial);
    }
}
