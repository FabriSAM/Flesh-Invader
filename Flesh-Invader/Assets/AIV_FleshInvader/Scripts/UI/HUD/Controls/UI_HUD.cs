using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UI_HUD : MonoBehaviour
{
    private VisualElement root;

    public void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement;
        InputManager.UI.PauseDisable.performed += OnPauseMenuClose;
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PlayerDeath, OnDisactiveHUD);
        GlobalEventSystem.AddListener(EventName.PlayerWin, OnDisactiveHUD);
        GlobalEventSystem.AddListener(EventName.PauseMenuEvent, OnDisactiveHUD);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnDisactiveHUD);
        GlobalEventSystem.RemoveListener(EventName.PlayerWin, OnDisactiveHUD);
        GlobalEventSystem.RemoveListener(EventName.PauseMenuEvent, OnDisactiveHUD);
    }

    private void OnDisactiveHUD(EventArgs message) {
        root.style.display = DisplayStyle.None;
    }

    private void OnPauseMenuClose(InputAction.CallbackContext context) {
        root.style.display = DisplayStyle.Flex;
    }
}
