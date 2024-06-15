using NotserializableEventManager;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_EndScreen : MonoBehaviour {

    private VisualElement root;
    private VisualElement statistics;
    private Label title;
    private Label statisticsTitle;
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
        title = root.Q<Label>("title");
        statisticsTitle = root.Q<Label>("statistics-title");
        time = root.Q<Label>("time-value");
        mission = root.Q<Label>("mission-value");
        possessionSuccess = root.Q<Label>("possessions-success-value");
        possessionFailed = root.Q<Label>("possessions-failed-value");
        bulletsFired = root.Q<Label>("bullets-fired-value");
        retry = root.Q<Button>("retry");
        mainMenu = root.Q<Button>("main-menu");
        statistics = root.Q<VisualElement>("statistics-container");
    }

    private void Start() {
        retry.clickable.clicked += delegate {
            SceneManager.LoadScene(1);
        };
        mainMenu.clickable.clicked += delegate {
            SceneManager.LoadScene(0);
        };
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PlayerDeath, OnPlayerDeath);
        GlobalEventSystem.AddListener(EventName.PlayerWin, OnPlayerWin);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnPlayerDeath);
        GlobalEventSystem.RemoveListener(EventName.PlayerWin, OnPlayerWin);
    }

    private void OnPlayerDeath(EventArgs message) {
        SwitchInputMap();
        EventArgsFactory.PlayerDeathParser(message, out Statistics statistics);
        WriteStatistics(statistics);
        SetSpecificEndScreenInfo("Mission failed!", Color.red, "navicella-rotta.jpg");
        ShowEndScreen();
    }

    private void OnPlayerWin(EventArgs message) {
        SwitchInputMap();
        EventArgsFactory.PlayerWinParser(message, out Statistics statistics);
        WriteStatistics(statistics);
        SetSpecificEndScreenInfo("Mission success!", Color.green, "navicella-riparata.jpg");
        ShowEndScreen();
    }

    private void SwitchInputMap() {
        InputManager.EnablePlayerMap(false);
        InputManager.EnableUIMap(true);
    }

    private void SetSpecificEndScreenInfo(string titleLabel, Color titleColor, string background) {
        title.text = titleLabel;
        statisticsTitle.style.color = titleColor;
        title.style.color = titleColor;
        root.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AIV_FleshInvader/Images/EndScreen/" + background);
    }

    private void WriteStatistics(Statistics statistics) {
        int totalSecondsInt = (int)statistics.GameTime;
        int hours = totalSecondsInt / 3600;
        totalSecondsInt %= 3600;
        int minutes = totalSecondsInt / 60;
        int seconds = totalSecondsInt % 60;
        time.text = $"{hours}h {minutes}m {seconds}s";
        mission.text = $"{statistics.CollectiblesFound.CurrentObject} of {statistics.CollectiblesFound.MaxObject}";
        possessionSuccess.text = statistics.PossessionSuccess.ToString();
        possessionFailed.text = statistics.PossessionFailed.ToString();
        bulletsFired.text = statistics.BulletFired.ToString();
    }

    private void ShowEndScreen() {
        root.style.display = DisplayStyle.Flex;
        StartCoroutine(ChangeBorderColor());
    }

    private IEnumerator ChangeBorderColor() {
        bool green = true;
        while (true) {
            yield return new WaitForSecondsRealtime(0.35f);
            green = !green;
            statistics.style.borderTopColor = green ? Color.green : Color.red;
            statistics.style.borderRightColor = green ? Color.green : Color.red;
            statistics.style.borderBottomColor = green ? Color.green : Color.red;
            statistics.style.borderLeftColor = green ? Color.green : Color.red;
        }
    }
}
