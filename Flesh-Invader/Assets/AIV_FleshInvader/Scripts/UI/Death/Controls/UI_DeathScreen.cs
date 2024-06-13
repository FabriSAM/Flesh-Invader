using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_DeathScreen : MonoBehaviour
{
    private VisualElement root;
    private Label time;
    private Label mission;

    private void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement.Q("root");
        root.style.display = DisplayStyle.None;
        time = root.Q<Label>("time-value");
        mission = root.Q<Label>("mission-value");
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PlayerDeath, OnPlayerDeath);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnPlayerDeath);
    }

    private void OnPlayerDeath(EventArgs message) {
        EventArgsFactory.PlayerDeathParser(message, out Statistics statistics);
        float hours = statistics.GameTime / 3600;
        int minutes = (int)((statistics.GameTime % 3600) / 60);
        int seconds = (int)((statistics.GameTime % 3600) % 60);
        time.text = $"{hours}h {minutes}m {seconds}s";
    }
}
