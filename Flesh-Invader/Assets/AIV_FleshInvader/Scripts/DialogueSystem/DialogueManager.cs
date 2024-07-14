using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    [SerializeField]
    private DialogueDatabase database;

    private IDisplayer displayer;

    private DialogueEntry currentDisplayedEntry;
    private uint currentDialogueID;


    #region Mono
    private void Awake() {
        displayer = GetComponent<IDisplayer>();
    }

    private void OnEnable() {
        GlobalEventSystem.AddListener(EventName.StartDialogue,
            OnStartDialogue);
    }

    private void OnDisable() {
        GlobalEventSystem.RemoveListener(EventName.StartDialogue,
            OnStartDialogue);
    }
    #endregion

    #region InterfaceComunication
    private void OpenUI() {
        displayer.Open();
    }

    private void CloseUI () {
        displayer.Close();
    }

    private void DisplayText (string text) {
        displayer.DisplayEntry(text);
    }
    #endregion


    #region Callbacks
    private void OnStartDialogue (EventArgs message) {
        EventArgsFactory.StartDialogueParser(message, out uint dialogueID, out int entryID);
        currentDisplayedEntry = database.GetEntry(dialogueID, entryID);
        if (!CanEntryBeDisplayed(currentDisplayedEntry)) return; //throw an undisplayable entry event
        StartDialogue();
    }

    private void OnEntryDisplayed () {
        displayer.OnEntryDisplayed -= OnEntryDisplayed;
        if (currentDisplayedEntry.NextEntry_ID == -1) {
            EndDialogue();
            return;
        }

        currentDisplayedEntry = database.GetEntry(currentDialogueID,
            (int)currentDisplayedEntry.NextEntry_ID);
        if (!CanEntryBeDisplayed(currentDisplayedEntry)) {
            EndDialogue();
            return;
        }
        DisplayEntry();
    }
    #endregion

    private bool CanEntryBeDisplayed (DialogueEntry entry) {
        if (entry == null) return false;
        //logic to know if can be displayed
        return true;
    }

    private void StartDialogue () {
        OpenUI();
        currentDialogueID = currentDisplayedEntry.Dialogue_ID;
        DisplayEntry();
    }

    private void DisplayEntry () {
        displayer.OnEntryDisplayed += OnEntryDisplayed;
        DisplayText(currentDisplayedEntry.Dialogue_Text);
    }

    private void EndDialogue () {
        CloseUI();
        GlobalEventSystem.CastEvent(EventName.DialoguePerformed,
            EventArgsFactory.DialoguePerformedFactory(currentDialogueID));
    }

}
