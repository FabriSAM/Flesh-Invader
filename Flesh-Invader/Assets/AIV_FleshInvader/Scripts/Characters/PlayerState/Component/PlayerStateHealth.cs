using NotserializableEventManager;
using UnityEngine;

public class PlayerStateHealth : MonoBehaviour
{
    #region Const
    private const float possessionHpMultiplier = 0.5f;
    #endregion

    #region SerializedField
    [Header("Health related parameters")]
    [SerializeField]
    private float defaultMaxHp;
    [SerializeField]
    private float defaultConstantDamage;
    [SerializeField]
    private float defaultMaxTimer;

    [Header("CameraShake parameters")]
    [SerializeField]
    private float amplitude;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool overrideCameraShakeCoroutine;
    #endregion

    #region Variable
    private float reduceTimer;
    private float currentHP;
    private bool deadStatus;
    private float maxHP;
    private float maxTimer;
    private float constantDamage;

    public bool DeadStatus {  get { return deadStatus; } }
    #endregion

    #region Mono
    void Update()
    {
        reduceTimer -= Time.deltaTime;

        if (reduceTimer < 0)
        {
            HealthReduce(constantDamage);
            SendCameraShakeEvent();
            reduceTimer = maxTimer;
        }
    }
    #endregion

    #region PublicMehtods
    public void HealthReduce(float damage)
    {
        if (deadStatus) { return; }
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);
        SendMessageHealthUpdate();

        if (currentHP <= 0)
        {
            PlayerDeathAnimation();
        }
    }

    public void HealthAdd(float healthToAdd)
    {
        currentHP = Mathf.Clamp(currentHP + healthToAdd, 0, maxHP);
        SendMessageHealthUpdate();
    }

    public void SetHealthForPossession()
    {
        HealthAdd(maxHP * possessionHpMultiplier);
    }

    public void PlayerDeathAnimation()
    {
        deadStatus = true;
        SendMessagePlayerDeathAnimationStart();
    }

    public void PlayerDeath()
    {
        SendMessagePlayerDeath();
    }

    public void HealthReset()
    {
        deadStatus = false;
        maxHP = defaultMaxHp;
        currentHP = maxHP;
    }

    public void HealthSet(float health)
    {
        currentHP = health;
        SendMessageHealthUpdate();
    }

    public float getCurrentHealth()
    {
        return currentHP;
    }

    public void HealthDamageTimerReset()
    {
        maxTimer = defaultMaxTimer;
        reduceTimer = maxTimer;
        constantDamage = defaultConstantDamage;
    }

    public void InitMe(PlayerState playerState)
    {
        //if(StaticLoading.LoadSaveGame)
        HealthReset();
        HealthDamageTimerReset();
        playerState.LevelController.OnLevelChange += OnLevelChange;
        SendMessageHealthUpdate();
    }
    #endregion

    #region PrivateMethods
    private void OnLevelChange(int value)
    {
        // Balance new stats when level up
        maxHP = defaultMaxHp * value;
        maxTimer = defaultMaxTimer * value;
        constantDamage = defaultConstantDamage;

        SendMessageHealthUpdate();
    }

    private void SendCameraShakeEvent()
    {
        GlobalEventSystem.CastEvent(EventName.CameraShake,
            EventArgsFactory.CameraShakeFactory(amplitude, duration, overrideCameraShakeCoroutine));
    }

    private void SendMessageHealthUpdate()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated,
            EventArgsFactory.PlayerHealthUpdatedFactory(maxHP, currentHP));
    }

    private void SendMessagePlayerDeathAnimationStart()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerDeathAnimationStart,
            EventArgsFactory.PlayerDeathAnimationStartFactory());
    }
    private void SendMessagePlayerDeath()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerDeath,
            EventArgsFactory.PlayerDeathFactory(PlayerState.Get().InformationController.GetStats()));
    }
    #endregion
}
