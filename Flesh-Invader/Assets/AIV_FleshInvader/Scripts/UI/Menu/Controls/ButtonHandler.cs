using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ButtonsHandler : MonoBehaviour
{
    Button quitButton;
    Button newGameButton;
    VisualElement rootVisualElement;

    private void OnEnable() {
        UIDocument uiDocument = GetComponent<UIDocument>();
        rootVisualElement = uiDocument.rootVisualElement;

        quitButton = rootVisualElement.Q<Button>("QuitButton");
        quitButton.clicked += OnQuitBottonClicked;

        newGameButton = rootVisualElement.Q<Button>("NewGameButton");
        newGameButton.clicked += OnNewGameButtonClicked;
    }

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

        while (!asyncOperation.isDone && loadingBar != null) {
            float progress = asyncOperation.progress;
            loadingBar.value = progress;
            loadingBar.title = $"{progress}%";
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    private void OnQuitBottonClicked() {
        Application.Quit();
    }
}