using NotserializableEventManager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class UI_MainMenu : MonoBehaviour
{
    Button quitButton;
    Button newGameButton;
    Button continueButton;

    VisualElement rootVisualElement;

    private void OnEnable() {
        UIDocument uiDocument = GetComponent<UIDocument>();
        rootVisualElement = uiDocument.rootVisualElement;

        quitButton = rootVisualElement.Q<Button>("QuitButton");
        quitButton.clicked += OnQuitBottonClicked;

        newGameButton = rootVisualElement.Q<Button>("NewGameButton");
        newGameButton.clicked += OnNewGameButtonClicked;

        continueButton = rootVisualElement.Q<Button>("ContinueButton");
        continueButton.clicked += OnContinueGameButtonClicked;

        if (!SaveSystem.GameDataExists(0))
        {
            continueButton.style.display = DisplayStyle.None;
        }
    }

    private void OnContinueGameButtonClicked()
    {
        StartCoroutine(LoadLevelCoroutine());

    }

    IEnumerator LoadLevelCoroutine()
    {
        var menuContainer = rootVisualElement.Q<VisualElement>("MenuContainer");
        menuContainer.style.display = DisplayStyle.None;
        var loadingContainer = rootVisualElement.Q<VisualElement>("LoadingContainer");
        loadingContainer.style.display = DisplayStyle.Flex;
        var loadingBar = rootVisualElement.Q<ProgressBar>("LoadingBar");

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);
        //yield return new WaitForSeconds(1);

        // Load datas from disk to code
        SaveSystem.LoadSlotData(0);

        while (!asyncOperation.isDone && loadingBar != null)
        {
            float progress = asyncOperation.progress;
            loadingBar.value = progress;
            loadingBar.title = $"{progress}%";
            yield return new WaitForEndOfFrame();
        } 

        //MenuInfo.ToLoad = true;
        //MenuInfo.EnemyToLoad = SaveSystem.ActiveGameData.PlayerSavedData.PlayerCharInfo;
        StaticLoading.LoadSaveGame = true;
        // Spawn character and load statistics
        // CharacterSpawner.GetInstance().LoadPlayerCharacter(SaveSystem.ActiveGameData.PlayerSavedData.PlayerCharInfo);

        yield return null;
    }

    #region NewGame
    private void OnNewGameButtonClicked() {
        //async load with loading widget
        StartCoroutine(ChangeLevelCoroutine());
    }

    IEnumerator ChangeLevelCoroutine() {
        var menuContainer = rootVisualElement.Q<VisualElement>("MenuContainer");
        menuContainer.style.display = DisplayStyle.None;
        var loadingContainer = rootVisualElement.Q<VisualElement>("LoadingContainer");
        loadingContainer.style.display = DisplayStyle.Flex;
        var loadingBar = rootVisualElement.Q<ProgressBar>("LoadingBar");

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);
        //yield return new WaitForSeconds(1);

        while (!asyncOperation.isDone && loadingBar != null) {
            float progress = asyncOperation.progress;
            loadingBar.value = progress;
            loadingBar.title = $"{progress}%";
            yield return new WaitForEndOfFrame();
        }
        SaveSystem.CreateGameData(0);

        yield return null;
    }
    #endregion

    private void OnQuitBottonClicked() {
        Application.Quit();
    }
}

//ToDo
/*
    continue
    new game
    settings {
        volume
        video
    }
    quit
 */