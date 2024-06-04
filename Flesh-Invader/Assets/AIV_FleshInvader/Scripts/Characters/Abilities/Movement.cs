using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : AbilityBase
{
    private const string xAxis = "XAxisValue";
    private const string zAxis = "ZAxisValue";
    private const string isMoving = "IsMoving";

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
        InternalUpdateAnimator(inputDirection);
    }

    private void Rotate()
    {
        if (cam == null) return;
        Vector3 mouse = InputManager.Player.MousePosition.ReadValue<Vector2>();
        Ray castPoint = cam.ScreenPointToRay(mouse);
        RaycastHit hit;
        if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Debug.DrawLine(cam.transform.position, hit.point, Color.red);
            Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            characterController.SetRotation(hitPoint, rotSpeed);
        }
    }

    private void CharacterMovement()
    {
        Move();
        Rotate();
    }

    private void InternalUpdateAnimator(Vector2 newMovement)
    {
        if (newMovement == Vector2.zero)
        {
            characterController.Visual.SetAnimatorParameter(isMoving, false);
        }
        else
        {
            characterController.Visual.SetAnimatorParameter(isMoving, true);
        }

        // Movement animator
        characterController.Visual.SetAnimatorParameter(xAxis, newMovement.x);
        characterController.Visual.SetAnimatorParameter(zAxis, newMovement.y);
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
        PlayerState.Get().GenericController.Pos2 += UnRegisterInput;
    }

    public override void UnRegisterInput()
    {
        PlayerState.Get().GenericController.Move -= CharacterMovement;
    }
    #endregion


   
}
