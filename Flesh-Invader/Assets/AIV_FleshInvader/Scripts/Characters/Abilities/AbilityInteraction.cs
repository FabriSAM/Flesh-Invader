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
    public override void StopAbility()
    {
        throw new System.NotImplementedException();
    }
    public override void Init(Controller characterController)
    {
        base.Init(characterController);
    }

    public override void RegisterInput()
    {
        PlayerState.Get().GenericController.Interact += Interaction;
    }

    public override void UnRegisterInput()
    {
        PlayerState.Get().GenericController.Interact += Interaction;
    }
    #endregion

    #region Callback
    private void Interaction()
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
