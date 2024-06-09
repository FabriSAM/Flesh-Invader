using NotserializableEventManager;
using System;
using UnityEngine;

public class PlayerStateLevel : MonoBehaviour
{
    #region Serialized Field
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
        if(level.CurrentXP + xpToAdd >= level.NextLevelXp)
        {
            level.CurrentXP = level.CurrentXP + xpToAdd - level.NextLevelXp;
            level.CurrentLevel++;
            LeveUp();
        }
        else
        {
            level.CurrentXP += xpToAdd;
        }

        GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated,
            EventArgsFactory.PlayerXPUpdatedFactory(level));
    }

    public int GetXP()
    {
        return level.CurrentLevel;
    }

    public void SetXPMultiplyer(float newMultyplier)
    {
        xpMultiplier = newMultyplier;
    }

    public void LeveUp()
    {
        OnLevelChange?.Invoke(level.CurrentLevel);
    }

    public void InitMe()
    {
        level.CurrentLevel = 1;
    }
    #endregion
}
