using NotserializableEventManager;
using System;
using UnityEngine;

public class Controller : MonoBehaviour, IPossessable
{

    #region FMOD
    private const string enemyDeathEventName = "Death";
    private const string enemyHurtEventName = "Hurt";
    private const string playerHurtEventName = "Hurt";

    private const string enemiesBankName = "Enemies";
    private const string playerBankName = "Player";
    #endregion

    #region Const
    private const string animatorDeadParameter = "Dead";
    private const float defaultPossesDamage = float.MaxValue;
    private const string possessionVFXName = "Possession";
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
    [SerializeField]
    private CharacterVFXMng characterVFXMng;
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
        get { return isPossessed; } set { isPossessed = value; }
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

    public CharacterVFXMng VFXMng { get { return characterVFXMng; } }

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
        GlobalEventSystem.RemoveListener(EventName.PlayerDeathAnimationStart, OnPlayerStateDeathAnimationStart);
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
            AudioManager.Get().PlayOneShot(playerHurtEventName, playerBankName);
        }
        else
        {
            AudioManager.Get().PlayOneShot(enemyHurtEventName, enemiesBankName);
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
        characterVFXMng.ActivateEffect(possessionVFXName);
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
        // Bind correct event to Death Animation callback
        RegisterDeathAnimationCallback(OnPlayerStateDeathAnimationEnd);
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
        GlobalEventSystem.AddListener(EventName.PlayerDeathAnimationStart, OnPlayerStateDeathAnimationStart);
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
        // Bind correct event to Death Animation callback
        RegisterDeathAnimationCallback(OnDeathAnimationEnd);
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
        GlobalEventSystem.RemoveListener(EventName.PlayerDeathAnimationStart, OnPlayerStateDeathAnimationStart);
        combatManager.OnPossessionChanged?.Invoke("PlayerDamager");
        OnCharacterUnpossessed?.Invoke();
    }
    #endregion

    #region Death
    private void RegisterDeathAnimationCallback(Action callback)
    {
        OnCharacterDeathEnd = (Action)Delegate.RemoveAll(OnCharacterDeathEnd, OnCharacterDeathEnd);
        OnCharacterDeathEnd += callback;
    }
    private void InternalOnDeath()
    {
        IsDead = true;
        AudioManager.Get().PlayOneShot(enemyDeathEventName, enemiesBankName);
        SetVelocity(Vector3.zero);
        characterPhysicsCollider.enabled = false;
        CharacterRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        playerStateLevel.SetXP(CharacterInfo.CharStats.Xp);
    }
    private void OnDeathAnimationEnd()
    {
        GlobalEventSystem.CastEvent(EventName.EnemyDeath, EventArgsFactory.EnemyDeathFactory());
        gameObject.SetActive(false);
    }
    private void OnPlayerStateDeathAnimationEnd()
    {
        InputManager.EnablePlayerMap(true);
        PlayerStateHealth.PlayerDeath();
    }
    private void OnPlayerStateDeathAnimationStart(EventArgs _)
    {
        InputManager.EnablePlayerMap(false);
        UnpossessionUnregisterInputs();
        AudioManager.Get().PlayOneShot(enemyDeathEventName, enemiesBankName);
        characterRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        characterPhysicsCollider.enabled = false;
        visual.SetAnimatorParameter(animatorDeadParameter, true);
    }
    #endregion
}
