using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class InteractableBase : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    protected Collider trigger;
    [SerializeField]
    protected GameObject canvas;

    [SerializeField]
    protected CollectibleInfo info;
    #endregion

    #region Variables
    protected Controller controller;
    protected bool alreadyUsed;
    #endregion

    #region Mono
    private void OnEnable()
    {
        alreadyUsed = false;
    }
    #endregion

    #region ProtectedMethods
    protected void SubscribeInteract()
    {
        controller.OnInteractPerformed += OnOpen;
    }
    protected void UnscribeInteract()
    {
        controller.OnInteractPerformed -= OnOpen;
    }
    protected abstract void OnOpen();
    protected void InternalOnTriggerEnter(Collider other, bool status)
    {
        if(!CanOpen(other)) return;

        canvas.SetActive(status);
        if (status)
        {
            SubscribeInteract();
        }
        else
        {
            UnscribeInteract();
        }
    }
    protected virtual bool CanOpen(Collider other)
    {
        if (alreadyUsed) return false;
        Controller tempController = other.GetComponent<Controller>();

        if (tempController == null) return false;
        controller = tempController;
        if (!controller.IsPossessed) return false;
        if (!controller.CharacterInfo.CharStats.CanLockpick) return false;

        return true;
    }
    #endregion
}
