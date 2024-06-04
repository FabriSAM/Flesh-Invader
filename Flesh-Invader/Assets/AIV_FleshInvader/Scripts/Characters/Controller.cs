using System;
using UnityEngine;

public class Controller : MonoBehaviour, IDamageable, IDamager, IPossessable
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
    protected HealthModule healthModule;
    [SerializeField]
    protected Visual visual;
    [SerializeField]
    protected bool isPossessed;
    #endregion //References


    #region PrivateAttributes
    private AbilityBase[] abilities;
    private PlayerStateHealth playerStateHealth;
    //private DamageContainer meleeContainer;
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

    public Action attack;
    public Action OnCharacterPossessed;
    public Action OnCharacterUnpossessed;
    public Action<DamageContainer> OnControllerDamageTaken;
    public Action OnControllerDeath;

    #region Mono
    private void Awake()
    {
        abilities = GetComponentsInChildren<AbilityBase>();
        playerStateHealth = PlayerState.Get().GetComponentInChildren<PlayerStateHealth>();
        healthModule.OnDamageTaken += OnInternalDamageTaken;
        healthModule.OnDeath += OnInternalDeath;

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

    //PORCATA DA CAMBIARE
    void OnEnable()
    {
        if (CharacterInfo != null)
        {
            healthModule.SetHP(CharacterInfo.CharStats.Health);
        }
    }

    void Start()
    {
        healthModule.SetHP(CharacterInfo.CharStats.Health);
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
        SetDamagerCollidersLayerType("EnemyDamager");

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
        SetDamagerCollidersLayerType("PlayerDamager");

        OnCharacterUnpossessed?.Invoke();
        //gameObject.SetActive(false);
    }
    #endregion

    #region Callbacks
    private void OnMeleeHitted(IDamageable otherDamageable, Vector3 hitPosition)
    {
        DamageContainer damage = new DamageContainer();
        damage.Damage = ((IPossessable)otherDamageable).CharacterInfo.CharStats.Damage;
        damage.Damager = this;

        // Need to specify a DamageContainer. Maybe add it to Database? Or set it later
        otherDamageable.TakeDamage(damage);
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
        if (IsPossessed)
        {
            if (playerStateHealth == null) return;
            playerStateHealth.HealthReduce(damage.Damage);
        }
        else
        {
            if (healthModule == null) return;
            healthModule.TakeDamage(damage);
        }
    }
    #endregion

    #region Health Module
    private void OnInternalDamageTaken(DamageContainer damage)
    {
        Debug.Log($"CUrrent HP: {healthModule.CurrentHP} - MaxHP : {healthModule.MaxHP}");
        OnControllerDamageTaken?.Invoke(damage);
    }
    private void OnInternalDeath()
    {
        PlayerState.Get().GetComponentInChildren<PlayerStateLevel>().SetXP(CharacterInfo.CharStats.Xp);
        gameObject.SetActive(false);
        OnControllerDeath?.Invoke();
    }

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
