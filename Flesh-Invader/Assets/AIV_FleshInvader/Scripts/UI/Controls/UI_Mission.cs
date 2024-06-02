using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Mission : MonoBehaviour
{
    private Label mission;

    private void Awake()
    {
        mission = GetComponent<UIDocument>().rootVisualElement.Q<Label>("message");
    }

    private void OnEnable()
    {
        GlobalEventSystem.AddListener(EventName.MissionUpdated, OnMissionUpdated);
    }

    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.MissionUpdated, OnMissionUpdated);
    }

    private void OnMissionUpdated(EventArgs message)
    {
        EventArgsFactory.MissionUpdatedParser(message, out int maxValue, out int currentValue);
        mission.text = $"Hai Raccolto {currentValue.ToString()} di {maxValue.ToString()}";
    }
}
