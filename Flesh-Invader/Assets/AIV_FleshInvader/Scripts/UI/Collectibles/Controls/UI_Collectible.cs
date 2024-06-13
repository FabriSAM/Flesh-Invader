using NotserializableEventManager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UI_Collectible : MonoBehaviour, IDisplayer {

    [SerializeField]
    private float typeWriteSpeed;

    private VisualElement root;
    private VisualElement icon;
    private VisualElement container;
    private Label dialogue;
    private Label title;
    private Label collectibleFound;
    private Button button;
    private Coroutine typeWriteCoroutinRunning;
    private string currentText;

    #region Mono

    private void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement.Q("root");
        root.style.display = DisplayStyle.None;
        dialogue = root.Q<Label>("dialogue");
        title = root.Q<Label>("title");
        collectibleFound = root.Q<Label>("collectibles-found");
        icon = root.Q<VisualElement>("icon");
        container = root.Q<VisualElement>("container");
        button = root.Q<Button>("close");
        button.clicked += Close;
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.MissionUpdated, OnMissionUpdated);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.MissionUpdated, OnMissionUpdated);
    }

    #endregion

    #region IDisplayer

    public void Open() {
        InputManager.EnablePlayerMap(false);
        InputManager.EnableUIMap(true);
        InputManager.UI.DialogueSkip.performed += OnDialogueSkip;
        root.style.display = DisplayStyle.Flex;
        StartCoroutine(ChangeBorderColor());
    }

    public void Close() {
        //switch map
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);
        //hide root
        root.style.display = DisplayStyle.None;
        StopCoroutine(ChangeBorderColor());
    }

    public Action OnEntryDisplayed {
        get;
        set;
    }

    public void DisplayEntry(string text) {
        currentText = text;
        typeWriteCoroutinRunning = StartCoroutine(TypeWriteEffect());
    }

    #endregion

    #region Internal
    
    private IEnumerator TypeWriteEffect () {
        int i = 2;
        dialogue.text = currentText.Substring(0, i);
        while (i <= currentText.Length) {
            yield return new WaitForSeconds(typeWriteSpeed);
            dialogue.text = currentText.Substring(0, i);
            i++;
        }
        typeWriteCoroutinRunning = null;
    }

    private IEnumerator ChangeBorderColor() {
        bool green = true;
        while (true) {
            yield return new WaitForSecondsRealtime(0.35f);
            green = !green;
            container.style.borderTopColor = green ? Color.green : Color.red;
            container.style.borderRightColor = green ? Color.green : Color.red;
            container.style.borderBottomColor = green ? Color.green : Color.red;
            container.style.borderLeftColor = green ? Color.green : Color.red;
        }
    }

    private void OnDialogueSkip(InputAction.CallbackContext obj) {
        if (typeWriteCoroutinRunning != null) {
            StopCoroutine(typeWriteCoroutinRunning);
            typeWriteCoroutinRunning = null;
            dialogue.text = currentText;
            return;
        }
        OnEntryDisplayed?.Invoke();
    }

    private void OnMissionUpdated(EventArgs message) {
        EventArgsFactory.MissionUpdatedParser(message, out Collectible collectible);
        collectibleFound.text = $"{collectible.collectiblesFound.CurrentObject.ToString()} of {collectible.collectiblesFound.MaxObject.ToString()}";
        title.text = collectible.Info.Title;
        icon.style.backgroundImage = collectible.Info.Icon;
    }

    #endregion
}
