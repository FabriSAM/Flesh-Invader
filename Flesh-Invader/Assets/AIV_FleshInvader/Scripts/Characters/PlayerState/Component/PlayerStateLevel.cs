using NotserializableEventManager;
using System;
using UnityEngine;

public class PlayerStateLevel : MonoBehaviour
{
    #region Serialized Field
    [SerializeField]
    private float baseLevelXP;
    [SerializeField]
    private LevelStruct level;
    #endregion

    #region Action
    public Action<int> OnLevelChange;
    #endregion

    #region Variables
    private float xpMultiplier = 1f;
    #endregion

    #region PublicMehtdos
    public void SetXP(float xpToAdd)
    {
        xpToAdd *= xpMultiplier;
        if (level.CurrentXP + xpToAdd >= level.NextLevelXp)
        {
            level.CurrentXP = level.CurrentXP + xpToAdd - level.NextLevelXp;
            level.CurrentLevel++;
            LeveUp();
        }
        else
        {
            level.CurrentXP += xpToAdd;
        }

        SendMessage();
    }

    public int GetCurrentLevel()
    {
        return level.CurrentLevel;
    }

    public void SetXPMultiplyer(float newMultyplier)
    {
        xpMultiplier = newMultyplier;
    }

    public void LeveUp()
    {
        level.NextLevelXp = baseLevelXP * level.CurrentLevel;
        OnLevelChange?.Invoke(level.CurrentLevel);
    }

    public void InitMe()
    {
        level.CurrentLevel = 1;
        level.NextLevelXp = baseLevelXP;
        SendMessage();
    }

    public void SetLevel(LevelStruct newLevel)
    {
        level = newLevel;
        SendMessage();
    }

    public LevelStruct GetLevelStruct()
    {
        return level;
    }
    #endregion

    #region PrivateMethods
    private void SendMessage()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated,
                                    EventArgsFactory.PlayerXPUpdatedFactory(level));
    }
    #endregion
}
