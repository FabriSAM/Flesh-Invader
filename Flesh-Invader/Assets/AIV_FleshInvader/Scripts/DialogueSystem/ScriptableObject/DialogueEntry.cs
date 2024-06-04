using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_Entry", menuName = "DialogueSystem/Dialogue_Entry", order = 1)]
public class DialogueEntry : ScriptableObject
{

    [SerializeField]
    private uint dialogue_ID;
    [SerializeField]
    private uint entry_ID;
    [SerializeField]
    private string dialogue_Text;
    [SerializeField]
    private int nextEntry_ID;

    public uint Dialogue_ID {
        get { return dialogue_ID; }
    }
    public uint Entry_ID {
        get { return entry_ID; }
    }
    public string Dialogue_Text {
        get { return dialogue_Text; }
    }
    public int NextEntry_ID {
        get { return nextEntry_ID; }
    }

    public string ToCSVLine () {
        return string.Empty;
    }

#if UNITY_EDITOR
    public void SetText(string text) {
        this.dialogue_Text = text;
    }
    
    public void SetNextEntry_ID (int nextEntry_ID) {
        this.nextEntry_ID = nextEntry_ID;
    }

    public static DialogueEntry Facotry (uint dialogueID, uint entryID, string entryText, int nextEntry_ID) {
        DialogueEntry entry = new DialogueEntry();
        entry.dialogue_ID = dialogueID;
        entry.entry_ID = entryID;
        entry.dialogue_Text = entryText;
        entry.nextEntry_ID = nextEntry_ID;
        return entry;
    }

#endif

}
