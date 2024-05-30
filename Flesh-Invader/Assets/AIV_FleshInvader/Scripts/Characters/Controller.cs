using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Controller : MonoBehaviour
{
    #region References
    [SerializeField]
    protected Transform characterTransform;
    [SerializeField]
    protected Rigidbody characterRigidbody;
    [SerializeField]
    protected Collider characterPhysicsCollider;
    [SerializeField]
    protected bool isPossessed;
    [SerializeField]
    protected Camera characterCamera;
    #endregion //References

    #region PrivateAttributes
    private AbilityBase[] abilities;
    #endregion

    #region ReferenceGetter
    public Transform PlayerTransform
    {
        get { return characterTransform; }
    }
    public Collider PlayerPhysicsCollider
    {
        get { return characterPhysicsCollider; }
    }
    public Rigidbody CharacterRigidbody 
    { 
        get { return characterRigidbody; } 
    }
    public bool IsPossessed
    {
        get { return isPossessed; }
    }
    #endregion

    #region PlayerMovement
    public Vector2 ComputedDirection
    {
        get;
        set;
    }
    public Action OnWalkStarted;
    public Action OnWalkEnded;
    public Action<float> OnDirectionChanged;
    #endregion

    #region RigidbodyMethods
    public Vector3 GetVelocity()
    {
        return characterRigidbody.velocity;
    }

    public void SetVelocity(Vector3 velocity)
    {
        characterRigidbody.velocity = transform.TransformDirection(new Vector3(velocity.x, characterRigidbody.velocity.y, velocity.z));
    }

    public void SetImpulse(Vector3 impulse)
    {
        SetVelocity(Vector2.zero);
        characterRigidbody.AddForce(impulse, ForceMode.Impulse);
    }

    public void SetRotation(Vector3 targetPoint, float rotSpeed)
    {
        var lookPos = targetPoint - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookPos);
        lookRot.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, lookRot.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotSpeed);
    }
    #endregion

    #region Object Interactions
    public Action OnInteractPerformed;
    #endregion

    #region Mono
    private void Awake()
    {
        abilities = GetComponentsInChildren<AbilityBase>();
        foreach (var ability in abilities)
        {
            ability.Init(this);
        }
        if (isPossessed)
        {
            internalOnPosses();
        }
        else
        {
            internalOnUnposses();
        }
    }
    public void internalOnPosses()
    {
        isPossessed = true;
        PlayerState.Get().PlayerTransform = transform;
        foreach (var ability in abilities)
        {
            ability.enabled = true;
        }
    }
    public void internalOnUnposses()
    {
        isPossessed = false;
        foreach (var ability in abilities)
        {
            ability.enabled = false;
        }
    }
    #endregion
}
