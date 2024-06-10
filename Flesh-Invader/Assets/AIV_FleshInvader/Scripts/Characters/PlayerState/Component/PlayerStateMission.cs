using NotserializableEventManager;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMission : MonoBehaviour
{

    private Collectible collectible = new Collectible();

    private void Start()
    {
        CallGlobalEvent();
    }

    public void AddMe()
    {
        collectible.MaxObject++;
    }
    public void Collected(CollectibleInfo info)
    {
        collectible.CurrentObject++;
        collectible.Info = info;
        CallGlobalEvent();
    }

    private void CallGlobalEvent()
    {
        GlobalEventSystem.CastEvent(EventName.MissionUpdated,
                                    EventArgsFactory.MissionUpdatedFactory(collectible));
    }
}
