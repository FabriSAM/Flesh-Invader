using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityInteraction : AbilityBase
{
    #region ProtectedMembers
    protected InputAction moveAction;
    #endregion
    #region Override
    public override void OnInputDisabled()
    {
        throw new System.NotImplementedException();
    }
    public override void OnInputEnabled()
    {
        throw new System.NotImplementedException();
    }
    public override void StopAbility()
    {
        throw new System.NotImplementedException();
    }
    public override void Init(Controller characterController)
    {
        base.Init(characterController);
        InputManager.Player.Interact.performed += InteractionPerformed;
    }

    #endregion

    #region Callback
    private void InteractionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Interaction");
        StartInteract();
    }

    private void StartInteract()
    {
        characterController.OnInteractPerformed?.Invoke();
    }
    #endregion
}
