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
    }

    private void Rotate()
    {
        if (cam == null) return;
        Vector3 mouse = InputManager.Player.MousePosition.ReadValue<Vector2>();
        Ray castPoint = cam.ScreenPointToRay(mouse);
        RaycastHit hit;
        if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            characterController.SetRotation(hitPoint, rotSpeed);
        }
        
    }
    private void CharacterMovement()
    {
        Move();
        Rotate();
    }
    #endregion

    #region Override
    public override void StopAbility()
    {
        
    }
    public override void Init(Controller characterController)
    {
        base.Init(characterController);
    }

    public override void RegisterInput()
    {
        PlayerState.Get().GenericController.Move += CharacterMovement;
    }

    public override void UnRegisterInput()
    {
        PlayerState.Get().GenericController.Move -= CharacterMovement;
    }
    #endregion
}
