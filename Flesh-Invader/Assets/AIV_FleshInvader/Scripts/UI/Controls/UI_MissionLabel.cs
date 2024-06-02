using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_MissionLabel : MonoBehaviour
{
    private Label mission;

    private void Awake()
    {
        mission = GetComponent<UIDocument>().rootVisualElement.Q<Label>("mission-label");
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
        mission.text = $"Hai raccolto {currentValue.ToString()} di {maxValue.ToString()}";
    }
}
