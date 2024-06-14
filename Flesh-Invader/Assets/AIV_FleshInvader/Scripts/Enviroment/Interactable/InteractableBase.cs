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
        if (!other.TryGetComponent(out controller)) return false;
        //if (!other.TryGetComponent(out character)) return;
        if (!controller.IsPossessed) return false;
        if (!controller.CharacterInfo.CharStats.CanLockpick) return false;

        return true;
    }
    #endregion
}
