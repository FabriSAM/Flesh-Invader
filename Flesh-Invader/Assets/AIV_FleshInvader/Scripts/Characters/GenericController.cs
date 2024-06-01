using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GenericController : MonoBehaviour
{
    public Action Move;
    public Action Attack;
    public Action Interact;
    public Action Rotate;
    public Action Posses;
    // Start is called before the first frame update
    void Awake()
    {
        InputManager.Player.Interact.performed += InteractionPerformed;
    }

    // Update is called once per frame

    public void InteractionPerformed(InputAction.CallbackContext context)
    {
        Interact?.Invoke();
    }
    void FixedUpdate()
    {
        Move?.Invoke();
    }
}
