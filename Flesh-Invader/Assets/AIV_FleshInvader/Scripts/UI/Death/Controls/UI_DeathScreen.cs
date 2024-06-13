using NotserializableEventManager;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_DeathScreen : MonoBehaviour
{
    private VisualElement root;
    private Label time;
    private Label mission;
    private Label possessionSuccess;
    private Label possessionFailed;
    private Label bulletsFired;
    private Button retry;
    private Button mainMenu;

    private void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement.Q("root");
        root.style.display = DisplayStyle.None;
        time = root.Q<Label>("time-value");
        mission = root.Q<Label>("mission-value");
        possessionSuccess = root.Q<Label>("possessions-success-value");
        possessionFailed = root.Q<Label>("possessions-failed-value");
        bulletsFired = root.Q<Label>("bullets-fired-value");
        retry = root.Q<Button>("retry");
        mainMenu = root.Q<Button>("main-menu");
    }

    private void Start() {
        retry.clickable.clicked += delegate {
            //TODO: new game
        };
        mainMenu.clickable.clicked += delegate {
            //TODO: main menu
        };
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PlayerDeath, OnPlayerDeath);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnPlayerDeath);
    }

    private void OnPlayerDeath(EventArgs message) {
        EventArgsFactory.PlayerDeathParser(message, out Statistics statistics);
        int totalSecondsInt = (int)statistics.GameTime;
        int hours = totalSecondsInt / 3600;
        totalSecondsInt %= 3600;
        int minutes = totalSecondsInt / 60;
        int seconds = totalSecondsInt % 60;
        time.text = $"{hours}h {minutes}m {seconds}s";
        mission.text = $"{statistics.CollectiblesFound.CurrentObject} of {statistics.CollectiblesFound.MaxObject}";
        root.style.display = DisplayStyle.Flex;
        possessionSuccess.text = statistics.PossessionSuccess.ToString();
        possessionFailed.text = statistics.PossessionFailed.ToString();
        bulletsFired.text = statistics.BulletFired.ToString();
    }
}
