using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : InteractableBase, ICollectable
{
    #region SerializedField
    [SerializeField]
    protected GameObject gate;
    [SerializeField]
    public uint collectibleID;
    #endregion

    #region Variables
    private Coroutine open;

    public uint CollectibleID => collectibleID;
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
        open = StartCoroutine(OpenDoor());
        alreadyUsed = true;

        // 2 is the TutorialMap Index
        if (SceneManager.GetActiveScene().buildIndex == 2) return;
        SaveSystem.ActiveGameData.PlayerSavedData.UnlockCollectible((int)collectibleID);
    }
    #endregion

    #region PrivateMethods
    private void CompleteOpen()
    {
        StopCoroutine(open);
    }
    #endregion

    #region Coroutine
    IEnumerator OpenDoor()
    {
        canvas.SetActive(false);
        float x = 0;
        while (x != 6)
        {
            x = Mathf.Clamp(gate.transform.localPosition.x + .05f, 0, 6);
            gate.transform.localPosition = new Vector3(x, gate.transform.localPosition.y, gate.transform.localPosition.z);
            yield return new WaitForSeconds(.01f);
        }
        CompleteOpen();
    }

    public void AddMission()
    {
        throw new System.NotImplementedException();
    }

    public void Collect()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    private void Start()
    {
        // 2 is the TutorialMap Index
        if (SceneManager.GetActiveScene().buildIndex == 2) return;
        if (SaveSystem.ActiveGameData.PlayerSavedData.IsCollectibleUnlocked((int)collectibleID))
        {
            OnOpen();
        }
    }
}
