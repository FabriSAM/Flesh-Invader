using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    protected Collider trigger;
    [SerializeField]
    protected GameObject canvas;
    #endregion

    protected Controller controller;


    protected void SubscribeInteract()
    {
        controller.OnInteractPerformed += OnOpen;
    }
    protected void UnscribeInteract()
    {
        controller.OnInteractPerformed -= OnOpen;
    }
    protected abstract void OnOpen();
    protected abstract void InternalOnTriggerEnter(Collider other, bool status);
}
