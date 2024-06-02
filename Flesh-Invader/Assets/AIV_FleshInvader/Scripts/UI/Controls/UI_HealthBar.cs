using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_HealthBar : MonoBehaviour {

    private ProgressBar healthBar;

    public UI_HealthBar() { }

    private void Awake() {
        healthBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("health-bar");
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PlayerHealthUpdated, OnHealthUpdate);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PlayerHealthUpdated, OnHealthUpdate);
    }

    private void OnHealthUpdate(EventArgs message) {
        EventArgsFactory.PlayerHealthUpdatedParser(message, out float maxHP, out float currentHP);
        healthBar.value = Mathf.Clamp((currentHP / maxHP), 0, 1);
    }
}
