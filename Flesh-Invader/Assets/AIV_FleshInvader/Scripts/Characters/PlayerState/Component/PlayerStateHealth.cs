using NotserializableEventManager;
using UnityEngine;

public class PlayerStateHealth : MonoBehaviour
{
    #region Const
    private const float possessionHpMultiplier = 0.5f;
    #endregion

    #region SerializedField
    [SerializeField]
    private float defaultMaxHp;
    [SerializeField]
    private float constantDamage;
    [SerializeField]
    private float maxTimer;
    #endregion

    #region Variable
    private float reduceTimer;
    private float currentHP;
    private bool deadStatus;
    private float maxHP;

    public bool DeadStatus {  get { return deadStatus; } }
    #endregion

    #region Mono
    private void Awake()
    {
        HealthReset();
    }
    void Update()
    {
        reduceTimer -= Time.deltaTime;

        if (reduceTimer < 0)
        {
            HealthReduce(constantDamage);
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
            PlayerDeath();
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

    public void PlayerDeath()
    {
        deadStatus = true;
        SendMessagePlayerDeath();
    }

    public void HealthReset()
    {
        deadStatus = false;
        maxHP = defaultMaxHp;
    }

    public void InitMe(PlayerState playerState)
    {
        reduceTimer = maxTimer;
        currentHP = maxHP;
        playerState.LevelController.OnLevelChange += OnLevelChange;
        SendMessageHealthUpdate();
    }
    #endregion

    #region PrivateMethods
    private void OnLevelChange(int value)
    {
        maxHP = defaultMaxHp * value;
        maxTimer *= value;
        reduceTimer *= value;
        constantDamage = Mathf.Clamp(constantDamage - value, 1, constantDamage);
    }

    private void SendMessageHealthUpdate()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated,
            EventArgsFactory.PlayerHealthUpdatedFactory(maxHP, currentHP));
    }

    private void SendMessagePlayerDeath()
    {
        //GlobalEventSystem.CastEvent(EventName.PlayerDeath,
        //    EventArgsFactory.PlayerDeathFactory(new Statistics()));
    }
    #endregion
}
