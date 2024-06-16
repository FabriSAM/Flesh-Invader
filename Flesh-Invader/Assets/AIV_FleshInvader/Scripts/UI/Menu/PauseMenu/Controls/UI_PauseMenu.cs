using NotserializableEventManager;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_PauseMenu : MonoBehaviour
{
    private VisualElement root;
    private VisualElement HUDroot;
    //buttons
    private Button buttonContinue;
    private Button buttonMainMenu;
    private Button buttonQuit;
    private VisualElement sureQuit;
    private Button buttonConfirmQuit;
    private Button buttonCancelQuit;
    //statistics
    private VisualElement statistics;
    private Label time;
    private Label mission;
    private Label possessionSuccess;
    private Label possessionFailed;
    private Label bulletsFired;
    //portrait
    private VisualElement portrait;

    #region Mono

    public void Awake() {
        //root
        root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;
        HUDroot = transform.parent.Find("UI_HUD").GetComponent<UIDocument>().rootVisualElement;
        //buttons
        buttonContinue = root.Q<Button>("button-continue");
        buttonMainMenu = root.Q<Button>("button-main-menu");
        buttonQuit = root.Q<Button>("button-quit");
        buttonConfirmQuit = root.Q<Button>("button-confirm-quit");
        buttonCancelQuit = root.Q<Button>("button-cancel-quit");
        //statistics
        time = root.Q<Label>("time-value");
        mission = root.Q<Label>("mission-value");
        possessionSuccess = root.Q<Label>("possessions-success-value");
        possessionFailed = root.Q<Label>("possessions-failed-value");
        bulletsFired = root.Q<Label>("bullets-fired-value");
        //other
        sureQuit = root.Q<VisualElement>("are-you-sure-quit");
        sureQuit.style.visibility = Visibility.Hidden;
        portrait = root.Q<VisualElement>("portrait");
        statistics = root.Q<VisualElement>("statistics-container");
        InputManager.UI.PauseDisable.performed += OnPauseMenuClose;
    }

    private void OnPauseMenuClose(InputAction.CallbackContext context) {
        ClosePauseMenu();
    }

    public void Start() {
        buttonContinue.clicked += ClosePauseMenu;
        buttonMainMenu.clicked += OnMainMenuClick;
        buttonQuit.clicked += OnQuitClick;
        buttonConfirmQuit.clicked += OnConfirmQuitClick;
        buttonCancelQuit.clicked += OnCancelQuitClick;
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.PauseMenuEvent, OnPauseMenuOpen);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PauseMenuEvent, OnPauseMenuOpen);
    }

    #endregion

    #region Internal

    private void OnPauseMenuOpen(EventArgs message) {
        InputManager.EnablePlayerMap(false);
        InputManager.EnableUIMap(true);
        Time.timeScale = 0;
        EventArgsFactory.PauseMenuEventParser(message, out Statistics statistics);
        InitializeUI(statistics);
        root.style.display = DisplayStyle.Flex;
        StartCoroutine("ChangeBorderColor");
    }

    private void InitializeUI(Statistics statistics) {
        //statistics
        WriteStatistics(statistics);
        //Portrait
        Camera camera = GameObject.Find("PortraitCamera")?.GetComponent<Camera>();
        if (camera != null) {
            Background bg = new Background();
            bg.renderTexture = camera.targetTexture;
            StyleBackground sb = new StyleBackground(bg);
            portrait.style.backgroundImage = sb;
        }
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

    private void ClosePauseMenu() {
        root.style.display = DisplayStyle.None;
        sureQuit.style.visibility = Visibility.Hidden;
        HUDroot.style.display = DisplayStyle.Flex;
        StopCoroutine("ChangeBorderColor");
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);
        Time.timeScale = 1;
    }

    private void OnMainMenuClick() {
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);
        Time.timeScale = 1;
        sureQuit.style.visibility = Visibility.Hidden;
        SceneManager.LoadScene(0);
    }

    private void OnQuitClick() {
        sureQuit.style.visibility = Visibility.Visible;
    }

    private void OnCancelQuitClick() {
        sureQuit.style.visibility = Visibility.Hidden;
    }

    private void OnConfirmQuitClick() {
        Application.Quit();
    }

    #endregion

    #region Coroutine
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
    #endregion
}