using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.CM.Common.CmCallContext;

public class UI_XPBar : MonoBehaviour {

    private ProgressBar xpBar;
    private VisualElement progressVisualElement;
    private VisualElement root;
    private Label progressLabel;

    private void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement;
        xpBar = root.Q<ProgressBar>("xp-bar");
        progressVisualElement = xpBar.Q<VisualElement>(className: "unity-progress-bar__progress");
        progressLabel = xpBar.Q<Label>(className: "unity-progress-bar__title");
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
        progressLabel.text = $"{(int)level.CurrentXP}/{level.NextLevelXp}";
        progressVisualElement.style.backgroundColor = Color.Lerp(new Color(0, 0.694f, 1, 1), new Color(0, 0.149f, 1, 1.0f), xp);
    }
}
