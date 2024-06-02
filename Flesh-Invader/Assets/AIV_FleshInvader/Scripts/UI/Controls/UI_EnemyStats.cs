using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_EnemyStats : MonoBehaviour
{
    private VisualElement icon;
    private Label damages;
    private Label speed;

    private void Awake() {
        icon = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("icon");
        damages = GetComponent<UIDocument>().rootVisualElement.Q<Label>("damages");
        speed = GetComponent<UIDocument>().rootVisualElement.Q<Label>("speed");
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PossessionExecuted, PossessionExecuted);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PossessionExecuted, PossessionExecuted);
    }

    private void PossessionExecuted(EventArgs message) {
        Debug.Log("UI_EnemyStats PossessionExecuted");
        EventArgsFactory.PossessionExecutedParser(message, out EnemyInfo enemyInfo);
        damages.text = "Damages: " + enemyInfo.CharStats.Damage;
        speed.text = "Speed: " + enemyInfo.CharStats.Speed;
    }
}
