using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

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
    #endregion

    #region Properties
    Camera CameraMain { get { return Camera.main; } }
    #endregion

    #region PrivateMethods
    private void Move()
    {
        Vector2 inputDirection = InputManager.Player_Move;
        Vector3 directionMovement=(Vector3.right*inputDirection.x+Vector3.forward*inputDirection.y).normalized;
        characterController.SetVelocity(directionMovement*speed);
        Vector3 localInputDirection = transform.InverseTransformDirection(directionMovement).normalized;
        InternalUpdateAnimator(new Vector2(localInputDirection.x,localInputDirection.z));
    }

    private void Rotate()
    {
        if (CameraMain == null) return;
        Vector3 mouse = InputManager.Player.MousePosition.ReadValue<Vector2>();
        Ray castPoint = CameraMain.ScreenPointToRay(mouse);
        RaycastHit hit;
        if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Debug.DrawLine(CameraMain.transform.position, hit.point, Color.red);
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
    }

    public override void UnRegisterInput()
    {
        PlayerState.Get().GenericController.Move -= CharacterMovement;
    }
    #endregion 
}
