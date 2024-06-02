using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateLevel : MonoBehaviour
{
    [SerializeField]
    private LevelStruct level;
    
    private float xpMultiplier = 1f;

    public void SetXP(float xpToAdd)
    {
        xpToAdd *= xpMultiplier;
        if(level.CurrentXP + xpToAdd >= level.NextLevelXp)
        {
            level.CurrentXP = level.CurrentXP + xpToAdd - level.NextLevelXp;
            level.CurrentLevel++;
            
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
}
