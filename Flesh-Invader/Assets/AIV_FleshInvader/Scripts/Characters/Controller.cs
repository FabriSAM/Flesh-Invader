using System;
using UnityEngine;

public class Controller : MonoBehaviour, IPossessable
{
    #region References
    [SerializeField]
    protected Transform characterTransform;
    [SerializeField]
    protected Rigidbody characterRigidbody;
    [SerializeField]
    protected Collider characterPhysicsCollider;
    [SerializeField]
    protected Visual visual;
    [SerializeField]
    protected CombatManager combatManager;
    [SerializeField]
    protected bool isPossessed;
    #endregion //References


    #region PrivateAttributes
    private AbilityBase[] abilities;
    private PlayerStateHealth playerStateHealth;
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
    public PlayerStateHealth PlayerStateHealth
    {
        get { return playerStateHealth; }
    }
    public Visual Visual
    {
        get { return visual; }
    }
    public CombatManager CombatManager 
    { 
        get { return combatManager; } 
    }

    public EnemyInfo CharacterInfo { get; set; }

    public bool UnPossessable { get; set; }
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
        Debug.DrawLine(transform.position, transform.position + transform.forward * 200, Color.green);
        var lookPos = targetPoint - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookPos);
        lookRot.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, lookRot.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotSpeed);
    }
    #endregion

    #region Object Interactions
    public Action OnInteractPerformed;
    #endregion

    #region Actions
    public Action attack;
    public Action OnCharacterPossessed;
    public Action OnCharacterUnpossessed;
    #endregion


    #region Mono
    private void Awake()
    {
        abilities = GetComponentsInChildren<AbilityBase>();
        playerStateHealth = PlayerState.Get().GetComponentInChildren<PlayerStateHealth>();
        combatManager.OnPerceivedDamage += InternalOnPerceivedDamage;
        CombatManager.OnHealthModuleDeath += InternalOnDeath;

        foreach (var ability in abilities)
        {
            ability.Init(this);
        }
    }

    void OnEnable()
    {
        if (CharacterInfo != null)
        {
            combatManager.OnControllerEnabled?.Invoke(CharacterInfo.CharStats.Health);
        }
    }

    private void Start()
    {
        if (isPossessed)
        {
            InternalOnPosses();
        }
        else
        {
            InternalOnUnposses();
        }
    }
    #endregion

    #region Internal
    public void InternalOnPosses()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        isPossessed = true;
        PlayerState.Get().PlayerTransform = transform;
        foreach (var ability in abilities)
        {
            ability.RegisterInput();
        }

        combatManager.OnPossessionChanged?.Invoke("EnemyDamager");
        OnCharacterPossessed?.Invoke();
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

        combatManager.OnPossessionChanged?.Invoke("PlayerDamager");
        OnCharacterUnpossessed?.Invoke();
    }

    private void InternalOnPerceivedDamage(DamageContainer damage)
    {
        if (IsPossessed)
        {
            if (playerStateHealth == null) return;
            playerStateHealth.HealthReduce(damage.Damage); 
        }
        else
        {
            combatManager.OnControllerDamageTaken?.Invoke(damage);
        }
    }
    private void InternalOnDeath()
    {
        PlayerState.Get().GetComponentInChildren<PlayerStateLevel>().SetXP(CharacterInfo.CharStats.Xp);
        //Maybe PlayerState if dead call GlobalEventManager

        gameObject.SetActive(false);
    }
    #endregion

    #region Interface Methods
    public void Possess()
    {
        InternalOnPosses();
    }
    public void UnPossess()
    {
        InternalOnUnposses();
    }
    #endregion
  
}
