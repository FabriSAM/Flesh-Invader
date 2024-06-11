using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.InputSystem;

public class UI_Dialogue : MonoBehaviour, IDisplayer {

    [SerializeField]
    private float typeWriteSpeed;

    private VisualElement root;
    private Label dialogue;
    private Button button;
    private Coroutine typeWriteCoroutinRunning;
    private string currentText;

    #region Mono
    private void Awake() {
        //hide dialog 
        root = GetComponent<UIDocument>().rootVisualElement.Q("root");
        root.style.display = DisplayStyle.None;
        dialogue = root.Q<Label>("dialogue");
        button = root.Q<Button>("close");
        button.clicked += CloseUI;
    }

    private void CloseUI()
    {
        Close();
    }
    #endregion


    #region IDisplayer

    public void Open() {
        InputManager.EnablePlayerMap(false);
        InputManager.EnableUIMap(true);
        InputManager.UI.DialogueSkip.performed += OnDialogueSkip;
        root.style.display = DisplayStyle.Flex;
        
    }

    public void Close() {
        //switch map
        InputManager.EnablePlayerMap(true);
        InputManager.EnableUIMap(false);
        //hide root
        root.style.display = DisplayStyle.None;
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



    private void OnDialogueSkip(InputAction.CallbackContext obj) {
        if (typeWriteCoroutinRunning != null) {
            StopCoroutine(typeWriteCoroutinRunning);
            typeWriteCoroutinRunning = null;
            dialogue.text = currentText;
            return;
        }
        OnEntryDisplayed?.Invoke();
    }
}
