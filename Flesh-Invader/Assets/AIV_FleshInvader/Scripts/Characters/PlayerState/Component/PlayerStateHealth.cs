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
        SendMessage();
    }

    public void HeathAdd(float healthToAdd)
    {
        currentHP = Mathf.Clamp(currentHP + healthToAdd, 0, maxHP);
        SendMessage();
    }

    public void InitMe(PlayerState playerState)
    {
        reduceTimer = maxTimer;
        currentHP = maxHP;
        playerState.LevelController.OnLevelChange += OnLevelChange;
        SendMessage();
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

    private void SendMessage()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdated,
            EventArgsFactory.PlayerHealthUpdatedFactory(maxHP, currentHP));
    }
    #endregion
}
