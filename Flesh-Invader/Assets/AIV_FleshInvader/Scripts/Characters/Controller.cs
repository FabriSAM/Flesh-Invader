using NotserializableEventManager;
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

    #region RigidbodyMethods
    public Vector3 GetVelocity()
    {
        return characterRigidbody.velocity;
    }

    public void SetVelocity(Vector3 velocity)
    {
        characterRigidbody.velocity = new Vector3(velocity.x, characterRigidbody.velocity.y, velocity.z);
    }

    public void SetImpulse(Vector3 impulse)
    {
        SetVelocity(Vector3.zero);
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

    public Action attack;

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
            InternalOnPosses();
        }
        else
        {
            InternalOnUnposses();
        }
    }
    public void InternalOnPosses()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        isPossessed = true;
        PlayerState.Get().PlayerTransform = transform;
        foreach (var ability in abilities)
        {
            ability.RegisterInput();
        }
        Debug.Log("Possessed");
    }
    public void InternalOnUnposses()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        isPossessed = false;
        foreach (var ability in abilities)
        {
            ability.UnRegisterInput();
        }
    }
    #endregion
}
