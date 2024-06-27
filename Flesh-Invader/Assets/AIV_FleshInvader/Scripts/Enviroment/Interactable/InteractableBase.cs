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
    protected LayerMask interactableMask;

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
        GlobalEventSystem.AddListener(EventName.PossessionExecuted, OnPossessionExecuted);
    }
    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.PossessionExecuted, OnPossessionExecuted);
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
        if (((1 << other.gameObject.layer) & interactableMask.value) == 0) return false;
        if (alreadyUsed) return false;
        Controller tempController = other.GetComponent<Controller>();

        if (tempController == null) return false;
        controller = tempController;
        if (!controller.IsPossessed) return false;
        if (!controller.CharacterInfo.CharStats.CanLockpick) return false;

        return true;
    }
    #endregion

    #region Callbacks
    protected void OnPossessionExecuted(EventArgs _)
    {
        if (controller == null) return;
        UnscribeInteract();
    }
    #endregion
}
