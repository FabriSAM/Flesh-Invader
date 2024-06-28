using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : InteractableBase, ICollectable
{
    #region SerializeFields
    [SerializeField]
    private uint dialogueID;
    [SerializeField]
    public uint collectibleID;
    #endregion
    public uint CollectibleID => collectibleID;

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
        UnscribeInteract();
        Collect();

        SaveSystem.ActiveGameData.PlayerSavedData.UnlockCollectible((int)collectibleID);
        SaveSystem.SaveGameStats(PlayerState.Get().CurrentPlayer.transform.position);
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
        missionController = PlayerState.Get().MissionController;
        AddMission();
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2) return;
        if (SaveSystem.ActiveGameData.PlayerSavedData.IsCollectibleUnlocked((int)collectibleID))
        {
            Collect();
        }
    }
    #endregion
}
