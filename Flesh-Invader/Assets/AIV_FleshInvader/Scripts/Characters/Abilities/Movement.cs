using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : AbilityBase
{
    #region SerializedField
    [SerializeField]
    protected float speed;
    #endregion

    #region ProtectedMembers
    protected InputAction moveAction;
    protected bool wasWalking;
    #endregion

    #region PrivateMethods
    private void Move()
    {
        characterController.ComputedDirection = moveAction.ReadValue<Vector2>();
        Vector3 velocity = new Vector3 (characterController.ComputedDirection.x,0,characterController.ComputedDirection.y).normalized * speed;
        characterController.SetVelocity(velocity);
        //.ScreenToWorldPoint(Input.mousePosition);
    }
    #endregion

    #region Override
    public override void OnInputDisabled()
    {
        
    }

    public override void OnInputEnabled()
    {
        
    }
    public override void StopAbility()
    {
        
    }
    public override void Init(Controller characterController)
    {
        base.Init(characterController);
        moveAction = InputManager.Player.Movement;
    }
    private void Update()
    {
        Move();
    }
    #endregion
}
