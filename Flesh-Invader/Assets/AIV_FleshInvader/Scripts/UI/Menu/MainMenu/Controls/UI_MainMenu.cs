using NotserializableEventManager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_MainMenu : MonoBehaviour
{
    Button quitButton;
    Button newGameButton;
    Button continueButton;
    Button tutorialButton;

    VisualElement rootVisualElement;

    private void OnEnable() {
        UIDocument uiDocument = GetComponent<UIDocument>();
        rootVisualElement = uiDocument.rootVisualElement;

        continueButton = rootVisualElement.Q<Button>("ContinueButton");
        continueButton.clicked += OnContinueGameButtonClicked;
        continueButton.RegisterCallback<MouseOverEvent>(onHoverSound);

        quitButton = rootVisualElement.Q<Button>("QuitButton");
        quitButton.clicked += OnQuitBottonClicked;
        quitButton.RegisterCallback<MouseOverEvent>(onHoverSound);

        newGameButton = rootVisualElement.Q<Button>("NewGameButton");
        newGameButton.clicked += OnNewGameButtonClicked;
        newGameButton.RegisterCallback<MouseOverEvent>(onHoverSound);

        tutorialButton = rootVisualElement.Q<Button>("TutorialButton");
        tutorialButton.clicked += OnTutorialButtonClicked;
        tutorialButton.RegisterCallback<MouseOverEvent>(onHoverSound);

        if (!SaveSystem.GameDataExists(0))
        {
            continueButton.style.display = DisplayStyle.None;
        }
    }



    private void onHoverSound(MouseOverEvent ev ) {
        AudioManager.Get().PlayOneShot("ButtonHover", "UI");
    }

    #region LoadGame
    private void OnContinueGameButtonClicked()
    {
        AudioManager.Get().PlayOneShot("ButtonClick", "UI");
        StartCoroutine(LoadLevelCoroutine(1));
    }

    IEnumerator LoadLevelCoroutine(int sceneToLoad)
    {
        var menuContainer = rootVisualElement.Q<VisualElement>("MenuContainer");
        menuContainer.style.display = DisplayStyle.None;
        var loadingContainer = rootVisualElement.Q<VisualElement>("LoadingContainer");
        loadingContainer.style.display = DisplayStyle.Flex;
        var loadingBar = rootVisualElement.Q<ProgressBar>("LoadingBar");
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);

        // Load datas from disk to code
        SaveSystem.LoadSlotData(0);

        StaticLoading.ManageSceneLoading(true);

        while (!asyncOperation.isDone && loadingBar != null)
        {
            float progress = asyncOperation.progress;
            loadingBar.value = progress;
            loadingBar.title = $"{progress}%";
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    #endregion

    #region NewGame
    private void OnNewGameButtonClicked() {
        AudioManager.Get().PlayOneShot("ButtonClick", "UI");
        StartCoroutine(StartLevelCoroutine(1));
    }

    IEnumerator StartLevelCoroutine(int sceneToLoad) {
        var menuContainer = rootVisualElement.Q<VisualElement>("MenuContainer");
        menuContainer.style.display = DisplayStyle.None;
        var loadingContainer = rootVisualElement.Q<VisualElement>("LoadingContainer");
        loadingContainer.style.display = DisplayStyle.Flex;
        var loadingBar = rootVisualElement.Q<ProgressBar>("LoadingBar");

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);
        //yield return new WaitForSeconds(1);

        if (sceneToLoad == 1)
        {
            if (SaveSystem.GameDataExists(0))
                SaveSystem.DeleteGameData(0);
        
            SaveSystem.CreateGameData(0);
            StaticLoading.ManageSceneLoading(false);
        }
        GlobalEventSystem.CastEvent(EventName.LoadGameEnded, EventArgsFactory.LoadGameEndedFactory(false));


        while (!asyncOperation.isDone && loadingBar != null) {
            float progress = asyncOperation.progress;
            loadingBar.value = progress;
            loadingBar.title = $"{progress}%";
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    #endregion

    #region Tutorial
    private void OnTutorialButtonClicked()
    {
        AudioManager.Get().PlayOneShot("ButtonClick", "UI");
        StartCoroutine(StartLevelCoroutine(2));
    }

    #endregion
    private void OnQuitBottonClicked() {
        Application.Quit();
    }
}