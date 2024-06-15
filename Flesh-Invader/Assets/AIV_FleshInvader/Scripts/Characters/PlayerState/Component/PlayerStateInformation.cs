using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEngine;

public class PlayerStateInformation : MonoBehaviour
{
    Statistics stats = new Statistics();
    // Start is called before the first frame update


    #region PublicMethods
    public void OpenPauseMenu()
    {
        stats.GameTime = Time.time;
        stats.CollectiblesFound = PlayerState.Get().MissionController.Collectible.collectiblesFound;
        GlobalEventSystem.CastEvent(EventName.PauseMenuEvent, EventArgsFactory.PauseMenuEventFactory(stats));
        //PauseMenuButtonHandler.Instance.OnPauseMenuTriggerEvent?.Invoke();
    }

    public void BulletFired()
    {
        stats.BulletFired++;
    }
    public void PossessionSuccess()
    {
        stats.PossessionSuccess++;
    }
    public void PossessionFailed()
    {
        stats.PossessionFailed++;
    }

    #endregion
}
