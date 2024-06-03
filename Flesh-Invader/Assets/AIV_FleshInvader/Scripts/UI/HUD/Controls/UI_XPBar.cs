using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_XPBar : MonoBehaviour {

    private ProgressBar xpBar;

    public UI_XPBar() { }

    private void Awake() {
        xpBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("xp-bar");
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PlayerXPUpdated, OnXPUpdate);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PlayerXPUpdated, OnXPUpdate);
    }

    private void OnXPUpdate(EventArgs message) {
        EventArgsFactory.PlayerXPUpdatedParser(message, out LevelStruct level);
        
        xpBar.value = Mathf.Clamp((level.CurrentXP / level.NextLevelXp), 0, 1);
    }
}
