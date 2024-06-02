using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateLevel : MonoBehaviour
{
    private LevelStruct level;
    
    public void SetXP(int xpToAdd)
    {
        
        if(level.CurrentXP + xpToAdd > level.NextLevelXp)
        {
            level.CurrentXP = level.CurrentXP + xpToAdd - level.NextLevelXp;
            level.CurrentLevel++;
            return;
        }

        level.CurrentXP += xpToAdd;

        GlobalEventSystem.CastEvent(EventName.PlayerXPUpdated,
            EventArgsFactory.PlayerXPUpdatedFactory(level));
    }

    public int GetXP()
    {
        return level.CurrentLevel;
    }
}
