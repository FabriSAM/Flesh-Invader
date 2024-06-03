using NotserializableEventManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Controller : MonoBehaviour, IDamageable, IDamager
{
    #region References
    [SerializeField]
    protected Transform characterTransform;
    [SerializeField]
    protected Rigidbody characterRigidbody;
    [SerializeField]
    protected Collider characterPhysicsCollider;
    [SerializeField]
    protected MeleeCollider[] meleeColliders;
    [SerializeField]
    protected bool isPossessed;
    #endregion //References
    

    #region PrivateAttributes
    private AbilityBase[] abilities;
    private DamageContainer meleeContainer;
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
    public DamageContainer MeleeContainer
    {
        get { return meleeContainer; }
        protected set { meleeContainer = value; }
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

        foreach (MeleeCollider collider in meleeColliders)
        {
            collider.DamageableHitted += OnMeleeHitted;
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
        SetDamagerCollidersLayerType("EnemyDamager");
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
        SetDamagerCollidersLayerType("PlayerDamager");
    }
    #endregion

    #region Callbacks
    private void OnMeleeHitted(IDamageable otherDamageable, Vector3 hitPosition)
    {
        // Need to specify a DamageContainer. Maybe add it to Database? Or set it later
        otherDamageable.TakeDamage(meleeContainer);
    }
    #endregion

    #region Private Methods
    private void SetDamagerCollidersLayerType(string newLayer)
    {
        foreach (MeleeCollider collider in meleeColliders)
        {
            Collider currentCollider = collider.GetComponent<Collider>();
            if (currentCollider == null) continue;
            currentCollider.gameObject.layer = LayerMask.NameToLayer(newLayer);
        }
    }
    #endregion

    #region Interface Methods
    public void TakeDamage(DamageContainer damage)
    {
        
    }
    #endregion
}
