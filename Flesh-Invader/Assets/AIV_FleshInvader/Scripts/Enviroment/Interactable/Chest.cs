using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableBase, ICollectable
{
    #region SerializeFields
    [SerializeField]
    private uint dialogueID;
    #endregion

    #region Variables
    PlayerStateMission missionController;
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
        Collect();
        UnscribeInteract();
    }

    public void Collect()
    {
        missionController.Collected(info);
        gameObject.SetActive(false);
    }

    public void AddMission()
    {
        missionController.AddMe();
    }

    #endregion

    #region Mono
    void Awake()
    {
        missionController = PlayerState.Get().GetComponentInChildren<PlayerStateMission>();
        AddMission();
    }
    #endregion
}
