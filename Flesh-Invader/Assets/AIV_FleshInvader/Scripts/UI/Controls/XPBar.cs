using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class XPBar : MonoBehaviour {

    private ProgressBar xpBar;

    public XPBar() { }

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
        EventArgsFactory.PlayerXPUpdatedParser(message, out float maxXP, out float currentXP);
        xpBar.value = Mathf.Clamp((currentXP / maxXP), 0, 1);
    }
}
