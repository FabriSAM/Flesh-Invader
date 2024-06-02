using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_LevelLabel : MonoBehaviour
{
    private Label level;

    private void Awake()
    {
        level = GetComponent<UIDocument>().rootVisualElement.Q<Label>("level-label");
    }

    private void OnEnable()
    {
        GlobalEventSystem.AddListener(EventName.PlayerXPUpdated, PlayerXPUpdated);
    }

    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.PlayerXPUpdated, PlayerXPUpdated);
    }

    private void PlayerXPUpdated(EventArgs message)
    {
        EventArgsFactory.PlayerXPUpdatedParser(message, out LevelStruct levelStruct);
        level.text = $"Level: {levelStruct.CurrentLevel}";
    }
}
