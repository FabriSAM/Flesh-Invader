using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_XPBar : MonoBehaviour {

    private ProgressBar xpBar;
    private VisualElement progressVisualElement;
    private VisualElement root;

    private void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement;
        xpBar = root.Q<ProgressBar>("xp-bar");
        progressVisualElement = xpBar.Q(className: "unity-progress-bar__progress");
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PlayerXPUpdated, OnXPUpdate);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PlayerXPUpdated, OnXPUpdate);
    }

    private void OnXPUpdate(EventArgs message) {
        EventArgsFactory.PlayerXPUpdatedParser(message, out LevelStruct level);
        float xp = Mathf.Clamp((level.CurrentXP / level.NextLevelXp), 0, 1);
        xpBar.value = xp;
        progressVisualElement.style.backgroundColor = Color.Lerp(new Color(0, 0.694f, 1, 1), new Color(0, 0.149f, 1, 1.0f), xp);
    }
}
