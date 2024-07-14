using NotserializableEventManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class Movement : AbilityBase
{
    #region Const
    // Animator
    private const string xAxis = "XAxisValue";
    private const string zAxis = "ZAxisValue";
    private const string isMoving = "IsMoving";

    // Consts
    private const float lowerStepRayCast = 1.0f;
    private const float higherStepRayCast = 1.5f;
    private const float groundedRayCast = 0.06f;
    private const string walkTrailVFXName = "WalkTrail";
    #endregion

    #region SerializedField
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float rotSpeed;

    [Header("StepUp Parameters")]
    [SerializeField]
    protected GameObject lowerStepUp;
    [SerializeField]
    protected GameObject upperStepUp;
    [SerializeField]
    protected float stepSmooth = 0.1f;
    #endregion

    #region PrivateMembers
    private Vector3 directionMovement;
    #endregion

    #region ProtectedMembers
    protected InputAction moveAction;
    protected bool wasWalking;
    #endregion

    #region Properties
    Camera CameraMain { get { return Camera.main; } }
    #endregion

    private void OnEnable()
    {
        GlobalEventSystem.AddListener(EventName.PossessionExecuted, OnPossess);
    }

    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.PossessionExecuted, OnPossess);
    }

    #region PrivateMethods
    private void Move()
    {
        Vector2 inputDirection = InputManager.Player_Move;
        
        directionMovement = (Vector3.right * inputDirection.x + Vector3.forward * inputDirection.y).normalized;
        characterController.SetVelocity(directionMovement * speed);
        Vector3 localInputDirection = transform.InverseTransformDirection(directionMovement).normalized;
        InternalUpdateAnimator(new Vector2(localInputDirection.x, localInputDirection.z));
    }

    private void Rotate()
    {
        if (CameraMain == null) return;
        Vector3 mouse = InputManager.Player.MousePosition.ReadValue<Vector2>();
        Ray castPoint = CameraMain.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Ground") | LayerMask.GetMask("Enemy")))
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
        StepClimb();
        CheckGrounded();
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

    private void StepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(lowerStepUp.transform.position, directionMovement.normalized, out hitLower, lowerStepRayCast))
        {
            RaycastHit hitHigher;
            if (!Physics.Raycast(upperStepUp.transform.position, directionMovement.normalized, out hitHigher, higherStepRayCast))
            {
                characterController.gameObject.transform.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }
    }

    private void CheckGrounded()
    {
        // Grounded
        RaycastHit grounded;
        if (Physics.Raycast(lowerStepUp.transform.position, -transform.up, out grounded, groundedRayCast))
        {
            characterController.CharacterRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            // Lock only rotation on Rigidbody
            characterController.CharacterRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
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

    public void OnPossess(EventArgs args)
    {
        EnemyInfo info;
        EventArgsFactory.PossessionExecutedParser(args, out info);

        speed = info.CharStats.BaseSpeed;
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
