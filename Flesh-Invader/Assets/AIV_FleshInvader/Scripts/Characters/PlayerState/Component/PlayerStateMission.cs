using NotserializableEventManager;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMission : MonoBehaviour
{

    private Collectible collectible = new Collectible();

    public Collectible Collectible { get { return collectible; } }

    public void InitMe()
    {        
        collectible.collectiblesFound.CurrentObject = 0;
        CallGlobalEvent();
        GlobalEventSystem.AddListener(EventName.UICollectableClose, CallWinMenu);
    }

    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.UICollectableClose, CallWinMenu);
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
    }

    private void CallGlobalEvent()
    {
        GlobalEventSystem.CastEvent(EventName.MissionUpdated,
            EventArgsFactory.MissionUpdatedFactory(collectible));
    }

    private void CallWinMenu(EventArgs _)
    {
        if (collectible.collectiblesFound.CurrentObject == collectible.collectiblesFound.MaxObject)
        {
            GlobalEventSystem.CastEvent(EventName.PlayerWin,
                EventArgsFactory.PlayerWinFactory(PlayerState.Get().InformationController.GetStats()));
        }
    }
}
