using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienObject : InteractableBase, ICollectable
{
    #region SerializeFields
    [SerializeField]
    private uint alienObjectID;    // Use this for saving
    [SerializeField] 
    public uint CollectibleID => alienObjectID;
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
        if (((1 << other.gameObject.layer) & interactableMask.value) == 0) return false;
        if (!other.TryGetComponent(out controller)) return false;
        //if (!other.TryGetComponent(out character)) return;
        if (!controller.IsPossessed) return false;

        return true;
    }

    protected override void OnOpen()
    {
        SaveSystem.ActiveGameData.PlayerSavedData.UnlockCollectible((int)alienObjectID);
        SaveSystem.SaveGameStats(PlayerState.Get().CurrentPlayer.transform.position);

        GlobalEventSystem.CastEvent(EventName.StartDialogue, EventArgsFactory.StartDialogueFactory(alienObjectID, 0));
        UnscribeInteract();
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
