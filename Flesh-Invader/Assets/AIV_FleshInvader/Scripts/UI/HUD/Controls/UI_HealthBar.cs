using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_HealthBar : MonoBehaviour {

    private ProgressBar healthBar;
    private VisualElement progressVisualElement;
    private VisualElement root;

    private void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement;
        healthBar = root.Q<ProgressBar>("health-bar");
        progressVisualElement = healthBar.Q(className: "unity-progress-bar__progress");
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PlayerHealthUpdated, OnHealthUpdate);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PlayerHealthUpdated, OnHealthUpdate);
    }

    private void OnHealthUpdate(EventArgs message) {
        EventArgsFactory.PlayerHealthUpdatedParser(message, out float maxHP, out float currentHP);
        float health = Mathf.Clamp((currentHP / maxHP), 0, 1);
        healthBar.value = health;
        progressVisualElement.style.backgroundColor = Color.Lerp(Color.red, new Color(0.102f, 0.812f, 0.106f, 1f), health);
    }
}
