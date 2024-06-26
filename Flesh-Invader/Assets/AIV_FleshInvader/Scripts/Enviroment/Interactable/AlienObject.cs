using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienObject : InteractableBase, ICollectable
{
    #region SerializeFields
    [SerializeField]
    private uint alienObjectID;    // Use this for saving
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
    protected override bool CanOpen(Collider other)
    {
        if (!other.TryGetComponent(out controller)) return false;
        //if (!other.TryGetComponent(out character)) return;
        if (!controller.IsPossessed) return false;

        return true;
    }

    protected override void OnOpen()
    {
        GlobalEventSystem.CastEvent(EventName.StartDialogue, EventArgsFactory.StartDialogueFactory(alienObjectID, 0));
        UnscribeInteract();

        SaveSystem.ActiveGameData.PlayerSavedData.UnlockCollectible((int)alienObjectID);
        
        Collect();
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
        // To move into Start?
        missionController = PlayerState.Get().MissionController;
        AddMission();
    }

    private void Start()
    {
        if (SaveSystem.ActiveGameData.PlayerSavedData.IsCollectibleUnlocked((int)alienObjectID))
        {
            Collect();
        }
    }

    #endregion

}
