using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion

    #region Mono
    private void Awake()
    {
        abilities = GetComponentsInChildren<AbilityBase>();
        foreach (var ability in abilities)
        {
            ability.Init(this);
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
    // Start is called before the first frame update
    void Start()
    {
        if (isPossessed)
        {
            internalOnPosses();
        }
        else
        {
            internalOnUnposses();
        }
    }
    #endregion
}
