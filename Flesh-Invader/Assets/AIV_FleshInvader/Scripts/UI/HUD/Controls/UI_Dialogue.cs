using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.InputSystem;

public class UI_Dialogue : MonoBehaviour, IDisplayer {


    [SerializeField]
    private float typeWriteSpeed;

    private VisualElement dialogueElement;
    private VisualElement root;
    private Label textBox;
    private Button button;
    private Coroutine typeWriteCoroutinRunning;
    private string currentText;

    #region Mono
    private void Awake() {
        //hide dialog 
        root = GetComponent<UIDocument>().rootVisualElement.Q("root");
        root.style.display = DisplayStyle.None;
        dialogueElement = root.Q("Baloon");
        textBox = dialogueElement.Q<Label>();
        button = dialogueElement.Q<Button>();
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
        //dialogueElement.visible = true;
        
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
        textBox.text = currentText.Substring(0, i);
        while (i <= currentText.Length) {
            yield return new WaitForSeconds(typeWriteSpeed);
            textBox.text = currentText.Substring(0, i);
            i++;
        }
        typeWriteCoroutinRunning = null;
    }



    private void OnDialogueSkip(InputAction.CallbackContext obj) {
        if (typeWriteCoroutinRunning != null) {
            StopCoroutine(typeWriteCoroutinRunning);
            typeWriteCoroutinRunning = null;
            textBox.text = currentText;
            return;
        }
        OnEntryDisplayed?.Invoke();
    }

}
