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
    [SerializeField]
    protected float rotSpeed;
    #endregion

    #region ProtectedMembers
    protected InputAction moveAction;
    protected bool wasWalking;
    private Camera cam;
    #endregion

    private void Start()
    {
        cam = Camera.main;
        
    }

    #region PrivateMethods
    private void Move()
    {
        Vector2 inputDirection = InputManager.Player_Move;
        Vector3 directionMovement=(transform.right*inputDirection.x+transform.forward*inputDirection.y).normalized;
        characterController.SetVelocity(directionMovement*speed);
        //characterController.ComputedDirection = moveAction.ReadValue<Vector2>();
        //Vector3 velocity = new Vector3 (characterController.ComputedDirection.x,0,characterController.ComputedDirection.y).normalized * speed;
        //characterController.SetVelocity(velocity);
    }

    private void Rotate()
    {
        Vector3 mouse = InputManager.Player.MousePosition.ReadValue<Vector2>();
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            characterController.SetRotation(hitPoint, rotSpeed);
        }
        
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
    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
    #endregion
}
