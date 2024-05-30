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
        characterController.ComputedDirection = moveAction.ReadValue<Vector2>();
        Vector3 velocity = new Vector3 (characterController.ComputedDirection.x,0,characterController.ComputedDirection.y).normalized * speed;
        characterController.SetVelocity(velocity);


    }

    private void Rotate()
    {
        Vector3 mouse = InputManager.Player.MousePosition.ReadValue<Vector2>();
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if(Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            Debug.Log(hitPoint);
            characterController.SetRotation(hitPoint, rotSpeed);
        }
        

        //Debug.Log("C2: " + mousePos.ToString());
        
    }
    #endregion

    //private void OnGUI()
    //{

    //    Vector3 point = new Vector3();
    //    Event currentEvent = Event.current;
    //    Vector2 mousePos = new Vector2();

    //    // Get the mouse position from Event.
    //    // Note that the y position from Event is inverted.
    //    mousePos.x = currentEvent.mousePosition.x;
    //    mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

    //    point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

    //    GUILayout.BeginArea(new Rect(20, 20, 250, 120));
    //    GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
    //    GUILayout.Label("Mouse position: " + mousePos);
    //    GUILayout.Label("World position: " + point.ToString("F3"));
    //    GUILayout.EndArea();

        
    //}

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
        Rotate();
    }
    #endregion
}
