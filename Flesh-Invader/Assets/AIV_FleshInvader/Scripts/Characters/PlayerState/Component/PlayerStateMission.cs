using NotserializableEventManager;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMission : MonoBehaviour
{

    private Collectible collectible = new Collectible();

    public Collectible Collectible {  get { return collectible; } } 

    public void InitMe()
    {
        CallGlobalEvent();
    }

    public void AddMe()
    {
        collectible.collectiblesFound.MaxObject++;
        CallGlobalEvent();
    }
    public void Collected(CollectibleInfo info)
    {
        collectible.collectiblesFound.CurrentObject++;
        collectible.Info = info;
        CallGlobalEvent();

        if (collectible.collectiblesFound.CurrentObject == collectible.collectiblesFound.MaxObject)
        {
            CallWinMenu();
        }
    }

    private void CallGlobalEvent()
    {
        GlobalEventSystem.CastEvent(EventName.MissionUpdated,
            EventArgsFactory.MissionUpdatedFactory(collectible));
    }

    private void CallWinMenu()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerWin,
            EventArgsFactory.PlayerWinFactory(PlayerState.Get().InformationController.GetStats()));
    }
}
