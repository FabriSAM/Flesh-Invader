using NotserializableEventManager;
using UnityEngine;

public class PlayerStateHealth : MonoBehaviour
{
    #region SerializedField
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float constantDamage;
    [SerializeField]
    private float maxTimer;
    #endregion

    #region Variable
    private float reduceTimer;
    private float currentHP;
    #endregion

    #region Mono

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

    public void PlayerDeath()
    {
        //PlayerState.Get().CurrentPlayer.
        SendMessagePlayerDeath();
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
        currentHP *= value;
        maxHP *= value;
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
        GlobalEventSystem.CastEvent(EventName.PlayerDeath,
            EventArgsFactory.PlayerDeathFactory(Time.time, PlayerState.Get().MissionController.Collectible.CurrentObject));
    }
    #endregion
}
