using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.InputSystem;

public class PauseMenuButtonHandler : MonoBehaviour
{
    public static PauseMenuButtonHandler Instance;

    VisualElement rootVisualElement;
    Button resumeButton;
    Button MainMenuButton;
    Button QuitGameButton;
    VisualElement ConfirmVE;
    Button ConfirmButton;
    Button CancelButton;

    Label bulletShotsLabel;
    Label stepsDoneLabel;
    Label successfulPossessionsLabel;
    Label failedPossessionsLabel;
    Label gameTimeLabel;

    VisualElement portraitVE;

    public bool IsPaused { 
        get => isPaused;
        set {
            isPaused = value;
            if (isPaused) {
                Time.timeScale = 0;
                InputManager.EnablePlayerMap(false);
                InputManager.EnableUIMap(true);
                rootVisualElement.style.display = DisplayStyle.Flex;
                successfulPossessionsLabel.text = $"Successful possessions {"null"}";
                bulletShotsLabel.text = $"Bullets shot: {"null"}";
                stepsDoneLabel.text = $"Steps done: {"null"}";
                failedPossessionsLabel.text = $"Failed possessions: {"null"}";
                double t = Math.Truncate(Time.time);
                gameTimeLabel.text = $"Game time: {Math.Truncate(t / 3600)}h:{Math.Truncate(t / 60)}m:{t}s";

            }
            else {
                Time.timeScale = 1;
                InputManager.EnablePlayerMap(true);
                InputManager.EnableUIMap(false);
                rootVisualElement.style.display = DisplayStyle.None;
            }
        }
    }
    private bool isPaused;
    private bool isInConfirm;

    public UnityEvent OnPauseMenuTriggerEvent;


    public void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (OnPauseMenuTriggerEvent == null) {
            OnPauseMenuTriggerEvent = new UnityEvent();
        }
        OnPauseMenuTriggerEvent.AddListener(OnPauseMenuToggle);

        InputManager.UI.PauseDisable.performed += DisablePause;

        InitializeUI();
    }

    private void InitializeUI() {
        UIDocument uiDocument = GetComponent<UIDocument>();
        rootVisualElement = uiDocument.rootVisualElement;

        resumeButton = rootVisualElement.Q<Button>("ContinueButton");
        resumeButton.clicked += OnResumeButtonClicked;

        MainMenuButton = rootVisualElement.Q<Button>("MainMenuButton");
        MainMenuButton.clicked += () => StartCoroutine(ToMainMenuCoroutine());

        QuitGameButton = rootVisualElement.Q<Button>("QuitGameButton");
        QuitGameButton.clicked += OnQuitButtonClicked;

        ConfirmVE = rootVisualElement.Q<VisualElement>("ConfirmVE");
        ConfirmVE.style.visibility = Visibility.Hidden;

        ConfirmButton = rootVisualElement.Q<Button>("ConfirmButton");
        ConfirmButton.clicked += OnConfirmButtonClicked;

        CancelButton = rootVisualElement.Q<Button>("CancelButton");
        CancelButton.clicked += OnCancelButtonConfirm;

        bulletShotsLabel = rootVisualElement.Q<Label>("BulletShots");
        stepsDoneLabel = rootVisualElement.Q<Label>("StepsDone");
        successfulPossessionsLabel = rootVisualElement.Q<Label>("SuccessfulPossessions");        
        failedPossessionsLabel = rootVisualElement.Q<Label>("FailedPossessions");
        gameTimeLabel = rootVisualElement.Q<Label>("GameTime");

        portraitVE = rootVisualElement.Q<VisualElement>("PortraitVE");
        Camera camera = GameObject.Find("PortraitCamera").GetComponent<Camera>();
        Background bg = new Background();
        bg.renderTexture = camera.targetTexture;
        StyleBackground sb = new StyleBackground(bg);
        portraitVE.style.backgroundImage = sb;

        IsPaused = false;
    }

    private void DisablePause(InputAction.CallbackContext context) {
        OnPauseMenuToggle();
    }

    private void OnPauseMenuToggle() {
        IsPaused = !IsPaused;
    }

    private void OnResumeButtonClicked() {
        OnPauseMenuToggle();
    }

    private IEnumerator ToMainMenuCoroutine() {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);

        while (!asyncOperation.isDone) {
            yield return new WaitForEndOfFrame();
        }

        IsPaused = false;

        yield return null;
    }

    private void OnQuitButtonClicked() {
        isInConfirm = !isInConfirm;
        ConfirmVE.style.visibility = (Visibility)Convert.ToInt32(!isInConfirm);
    }
    private void OnCancelButtonConfirm() {
        isInConfirm = false;
        ConfirmVE.style.visibility = Visibility.Hidden;
    }

    private void OnConfirmButtonClicked() {
        Application.Quit();
    }
}
//ToDo
/*
 * resume
 * main menu
 * settings
 * portrait of player
 * statistics (on the right) {
 *  bullet shot
 *  steps done
 *  possessions done/failed
 *  game time
 * }
 * 
 * esc input -> enable and disable input maps, and trigger the pause menu
 */