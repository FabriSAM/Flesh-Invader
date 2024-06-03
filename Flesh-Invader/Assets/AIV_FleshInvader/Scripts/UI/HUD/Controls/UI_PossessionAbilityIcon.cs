using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_PossessionAbilityIcon : MonoBehaviour
{
    private VisualElement icon;

    private void Awake() {
        icon = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("possession-ability-icon");
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PossessionAbilityState, PossessionAbilityState);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PossessionAbilityState, PossessionAbilityState);
    }

    private void PossessionAbilityState(EventArgs message) {
        EventArgsFactory.PossessionAbilityStateParser(message, out bool state);
        icon.style.unityBackgroundImageTintColor = state ? Color.white : Color.black;
    }
}
