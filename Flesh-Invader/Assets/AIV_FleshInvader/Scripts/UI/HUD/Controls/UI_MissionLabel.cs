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
        EventArgsFactory.MissionUpdatedParser(message, out Collectible collectible);
        mission.text = $"Found {collectible.collectiblesFound.CurrentObject.ToString()} of {collectible.collectiblesFound.MaxObject.ToString()}";
    }
}
