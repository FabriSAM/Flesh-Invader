using NotserializableEventManager;
using System;
using UnityEngine;

public class Controller : MonoBehaviour, IPossessable
{
    #region Const
    private const string animatorDeadParameter = "Dead";
    private const float defaultPossesDamage = float.MaxValue;
    #endregion

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
    [SerializeField]
    private SetOverlay overlay;
    [SerializeField]
    private Material overlayMaterial;
    #endregion //References

    #region PrivateAttributes
    private AbilityBase[] abilities;
    private PlayerStateHealth playerStateHealth;
    private PlayerStateLevel playerStateLevel;
    private PlayerStateInformation playerStateInformation;
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
    public SetOverlay Overlay { get { return overlay; } }

    public EnemyInfo CharacterInfo { get; set; }

    public bool UnPossessable { get; set; }
    public bool IsDead { get; set; }
    #endregion

    #region RigidbodyMethods
    public Vector3 GetVelocity()
    {
        return characterRigidbody.velocity;
    }
    public void SetVelocity(Vector3 velocity)
    {
        if (characterRigidbody.isKinematic) return;
        characterRigidbody.velocity = new Vector3(velocity.x, characterRigidbody.velocity.y, velocity.z);
    }
    public void SetImpulse(Vector3 impulse)
    {
        if (characterRigidbody.isKinematic) return;
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
    public void SetPosition(Vector3 newPos)
    {
        characterRigidbody.position = newPos;
    }
    private void SetRigidbodyParams(bool newIsKinematic, RigidbodyInterpolation newInterpolation)
    {
        CharacterRigidbody.isKinematic = newIsKinematic;
        CharacterRigidbody.interpolation = newInterpolation;
    }
    #endregion

    #region Actions
    // Attack
    public Action attack;
    // Possession
    public Action OnCharacterPossessed;
    public Action OnCharacterUnpossessed;
    // Death
    public Action OnCharacterDeathEnd;
    // Object Interaction
    public Action OnInteractPerformed;
    #endregion

    #region Mono
    private void Awake()
    {
        abilities = GetComponentsInChildren<AbilityBase>();
        playerStateHealth = PlayerState.Get().HealthController;
        playerStateLevel = PlayerState.Get().LevelController;
        playerStateInformation = PlayerState.Get().InformationController;
        if (isPossessed)
        {
            PlayerState.Get().CurrentPlayer = gameObject;
        }

        OnCharacterDeathEnd += OnDeathAnimationEnd;
        combatManager.OnPerceivedDamage += InternalOnPerceivedDamage;
        combatManager.OnHealthModuleDeath += InternalOnDeath;

        foreach (var ability in abilities)
        {
            ability.Init(this);
        }
    }

    void OnEnable()
    {
        characterPhysicsCollider.enabled = true;
        if (CharacterInfo != null)
        {
            IsDead = false;
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

    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnPlayerStateDeath);
    }
    #endregion

    #region Damage
    public void RequestDamage(DamageContainer damage = null)
    {
        if (damage == null)
        {
            damage = new DamageContainer();
            damage.Damage = defaultPossesDamage;
        }
        combatManager.TakeDamage(damage);
    }
    private void InternalOnPerceivedDamage(DamageContainer damage)
    {
        if (IsPossessed)
        {
            if (playerStateHealth == null) { return; }
            if (playerStateHealth.DeadStatus) { return; }
            playerStateHealth.HealthReduce(damage.Damage);
        }
        else
        {
            combatManager.OnControllerDamageTaken?.Invoke(damage);
        }
    }
    #endregion

    #region Possession
    // INTERFACE METHODS
    public void Possess()
    {
        playerStateInformation.PossessionSuccess();
        playerStateHealth.SetHealthForPossession();
        InternalOnPosses();
    }
    // INTERNAL METHODS
    private void InternalOnPosses()
    {
        //Setup parameters for possessd player
        PossessionSetupParams();
        //Setup inputs
        PossessionRegisterInputs();
        //Invoke all methods
        PossessionInvoke();
        //Setup overlay
        Overlay.AddOverlay(overlayMaterial);
    }
    private void PossessionSetupParams()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        isPossessed = true;
        PlayerState.Get().CurrentPlayer = gameObject;
        SetRigidbodyParams(false, RigidbodyInterpolation.Interpolate);
    }
    private void PossessionRegisterInputs()
    {
        foreach (var ability in abilities)
        {
            ability.RegisterInput();
        }
    }
    private void PossessionInvoke()
    {
        GlobalEventSystem.AddListener(EventName.PlayerDeath, OnPlayerStateDeath);
        combatManager.OnPossessionChanged?.Invoke("EnemyDamager");
        OnCharacterPossessed?.Invoke();
    }
    #endregion

    #region Unpossession
    // INTERFACE METHODS
    public void UnPossess()
    {
        InternalOnUnposses();
        RequestDamage();
    }
    // INTERNAL METHODS
    private void InternalOnUnposses()
    {         
        //Setup parameters for possessd player
        UnpossessionSetupParams();
        //Setup inputs
        UnpossessionUnregisterInputs();
        //Invoke all methods
        UnpossessionInvoke();
        //Setup overlay
        Overlay.RemoveOverlay(overlayMaterial);
    }
    private void UnpossessionSetupParams()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        isPossessed = false;
        SetRigidbodyParams(true, RigidbodyInterpolation.None);
    }
    private void UnpossessionUnregisterInputs()
    {
        foreach (var ability in abilities)
        {
            ability.UnRegisterInput();
        }
    }
    private void UnpossessionInvoke()
    {
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnPlayerStateDeath);
        combatManager.OnPossessionChanged?.Invoke("PlayerDamager");
        OnCharacterUnpossessed?.Invoke();
    }
    #endregion

    #region Death
    private void InternalOnDeath()
    {
        IsDead = true;
        SetVelocity(Vector3.zero);
        characterPhysicsCollider.enabled = false;
        CharacterRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        playerStateLevel.SetXP(CharacterInfo.CharStats.Xp);
    }
    private void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false);
        GlobalEventSystem.CastEvent(EventName.EnemyDeath, EventArgsFactory.EnemyDeathFactory());
    }
    private void OnPlayerStateDeath(EventArgs _)
    {
        UnpossessionUnregisterInputs();
        characterPhysicsCollider.enabled = false;
        visual.SetAnimatorParameter(animatorDeadParameter, true);
    }
    #endregion
}
