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
}
