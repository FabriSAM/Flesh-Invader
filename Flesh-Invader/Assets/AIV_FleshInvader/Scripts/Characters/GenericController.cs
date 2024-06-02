using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GenericController : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private float possessionCD;
    #endregion

    #region PrivateMembers
    private float currentTimeCache;
    #endregion

    #region Action
    public Action Move;
    public Action Attack;
    public Action Interact;
    public Action Rotate;
    public Action Posses;
    #endregion

    #region Mono
    void Awake()
    {
        currentTimeCache = -possessionCD;
        InputManager.Player.Interact.performed += InteractionPerformed;
        InputManager.Player.Possession.performed += PossessionPerformed;
    }
    void FixedUpdate()
    {
        Move?.Invoke();
    }
    #endregion

    #region InputCallBack
    private void PossessionPerformed(InputAction.CallbackContext context)
    {
        if (Time.time-currentTimeCache>=possessionCD)
        {
            currentTimeCache = Time.time;
            Posses?.Invoke();
        }
    }
    public void InteractionPerformed(InputAction.CallbackContext context)
    {
        Interact?.Invoke();
    }
    #endregion
}
