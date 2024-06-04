using NotserializableEventManager;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMission : MonoBehaviour
{
    private int maxObject;
    private int currentObject;

    private void Start()
    {
        CallGlobalEvent();
    }

    public void AddMe()
    {
        maxObject++;
    }
    public void Collected()
    {
        currentObject++;
        CallGlobalEvent();
    }

    private void CallGlobalEvent()
    {
        GlobalEventSystem.CastEvent(EventName.MissionUpdated,
                                    EventArgsFactory.MissionUpdatedFactory(maxObject, currentObject));
    }
}
