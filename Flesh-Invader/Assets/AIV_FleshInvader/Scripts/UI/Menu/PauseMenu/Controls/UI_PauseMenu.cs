using NotserializableEventManager;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_PauseMenu : MonoBehaviour
{
    #region SerializedField
    [SerializeField]
    private Transform[] modelPortraitLocation;
    [SerializeField]
    private Camera portraitCamera;
    #endregion

    #region InternalVariables
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
    //others
    private bool isPlayerDead;
    #endregion

    #region Mono
    public void Awake() {
        Debug.Log("UI_PauseMenu Awake" + isPlayerDead);
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
    }

    public void Start() {
        Debug.Log("UI_PauseMenu Start" + isPlayerDead);
        buttonContinue.clicked += ClosePauseMenu;
        buttonContinue.RegisterCallback<MouseOverEvent>(onHoverSound);
        buttonMainMenu.clicked += OnMainMenuClick;
        buttonMainMenu.RegisterCallback<MouseOverEvent>(onHoverSound);
        buttonQuit.clicked += OnQuitClick;
        buttonQuit.RegisterCallback<MouseOverEvent>(onHoverSound);
        buttonConfirmQuit.clicked += OnConfirmQuitClick;
        buttonConfirmQuit.RegisterCallback<MouseOverEvent>(onHoverSound);
        buttonCancelQuit.clicked += OnCancelQuitClick;
        buttonCancelQuit.RegisterCallback<MouseOverEvent>(onHoverSound);
    }

    private void OnEnable() {
        InputManager.UI.PauseDisable.performed += OnPauseMenuClose;
        GlobalEventSystem.AddListener(EventName.PauseMenuEvent, OnPauseMenuOpen);
        GlobalEventSystem.AddListener(EventName.PlayerDeath, OnPlayerDeath);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.PauseMenuEvent, OnPauseMenuOpen);
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnPlayerDeath);
        InputManager.UI.PauseDisable.performed -= OnPauseMenuClose;
    }

    #endregion

    #region Internal
    private void OnPauseMenuOpen(EventArgs message) {
        if (isPlayerDead) return;
        InputManager.EnablePlayerMap(false);
        InputManager.EnableUIMap(true);
        Time.timeScale = 0;
        EventArgsFactory.PauseMenuEventParser(message, out Statistics statistics);
        InitializeUI(statistics);
        root.style.display = DisplayStyle.Flex;
        StartCoroutine("ChangeBorderColor");
    }

    private void OnPlayerDeath(EventArgs message) {
        isPlayerDead = true;
    }

    private void OnPauseMenuClose(InputAction.CallbackContext context) {
        ClosePauseMenu();
    }

    private void InitializeUI(Statistics statistics) {
        //statistics
        WriteStatistics(statistics);
        //Portrait
        //portraitCamera = GameObject.Find("PortraitCamera")?.GetComponent<Camera>();
        if (portraitCamera != null) {
            Background bg = new Background();
            bg.renderTexture = portraitCamera.targetTexture;
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
        Debug.Log("CURRENT INDEX ENEMY" + statistics.CurrentIndexEnemy);
        SetPortraitCameraTransform(statistics.CurrentIndexEnemy);
    }
    
    private void SetPortraitCameraTransform(int enemyIndex)
    {
        Debug.Log(portraitCamera.transform);
        portraitCamera.transform.position = modelPortraitLocation[enemyIndex].position;
    }
    
    private void ClosePauseMenu() {
        AudioManager.Get().PlayOneShot("ButtonClick", "UI");
        root.style.display = DisplayStyle.None;
        sureQuit.style.visibility = Visibility.Hidden;
        HUDroot.style.display = DisplayStyle.Flex;
        StopCoroutine("ChangeBorderColor");
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);
        Time.timeScale = 1;
    }

    private void OnMainMenuClick() {
        AudioManager.Get().PlayOneShot("ButtonClick", "UI");
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);
        Time.timeScale = 1;
        sureQuit.style.visibility = Visibility.Hidden;
        SceneManager.LoadScene(0);
    }

    private void OnQuitClick() {
        AudioManager.Get().PlayOneShot("ButtonClick", "UI");
        sureQuit.style.visibility = Visibility.Visible;
    }

    private void OnCancelQuitClick() {
        AudioManager.Get().PlayOneShot("ButtonClick", "UI");
        sureQuit.style.visibility = Visibility.Hidden;
    }

    private void OnConfirmQuitClick() {
        Application.Quit();
    }

    private void onHoverSound(MouseOverEvent ev) {
        AudioManager.Get().PlayOneShot("ButtonHover", "UI");
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