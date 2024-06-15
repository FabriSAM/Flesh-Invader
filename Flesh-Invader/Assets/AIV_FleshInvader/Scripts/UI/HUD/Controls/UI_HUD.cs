using UnityEngine;

public class UI_HUD : MonoBehaviour
{
    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PlayerDeath, OnDisactiveHUD);
        GlobalEventSystem.AddListener(EventName.PlayerWin, OnDisactiveHUD);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnDisactiveHUD);
        GlobalEventSystem.RemoveListener(EventName.PlayerWin, OnDisactiveHUD);
    }

    private void OnDisactiveHUD(EventArgs message) {
        gameObject.SetActive(false);
    }
}
