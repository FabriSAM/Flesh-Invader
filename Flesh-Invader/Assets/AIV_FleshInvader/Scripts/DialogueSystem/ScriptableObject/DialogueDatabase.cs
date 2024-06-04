using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDatabase", menuName = "DialogueSystem/DialogueDatabase", order = 2)]
public class DialogueDatabase : ScriptableObject
{


    [SerializeField]
    private DialogueEntry[] entries;

    private Dictionary<uint, List<DialogueEntry>> sortedDialogues;

    public DialogueEntry GetEntry (uint dialogueID, int entryID) {
        if (sortedDialogues == null) {
            SortDialogues();
        }
        if (!sortedDialogues.ContainsKey(dialogueID)) return null;
        foreach (DialogueEntry entry in sortedDialogues[dialogueID]) {
            if (entry.Entry_ID == entryID) return entry;
        }
        return null;
    }


    private void SortDialogues () {
        sortedDialogues = new Dictionary<uint, List<DialogueEntry>>();
        foreach (DialogueEntry entry in entries) {
            if (!sortedDialogues.ContainsKey(entry.Dialogue_ID)) {
                sortedDialogues.Add(entry.Dialogue_ID, new List<DialogueEntry>());
            }
            sortedDialogues[entry.Dialogue_ID].Add(entry);
        }
    }
}
