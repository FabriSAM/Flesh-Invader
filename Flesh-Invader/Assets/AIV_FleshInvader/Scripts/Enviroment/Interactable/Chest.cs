using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableBase
{
    #region SerializeFields
    [SerializeField]
    private uint dialogueID;
    #endregion

    #region Callback
    private void OnTriggerEnter(Collider other)
    {
        InternalOnTriggerEnter(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        InternalOnTriggerEnter(other, false);
    }
    #endregion

    #region OverrideBaseClass
    protected override void OnOpen()
    {
        GlobalEventSystem.CastEvent(EventName.StartDialogue, EventArgsFactory.StartDialogueFactory(dialogueID, 0));
    }
    #endregion
}
