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
    #endregion

    protected Controller controller;
    protected EnemyChar character;


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
        if (!other.TryGetComponent(out controller)) return;
        //if (!other.TryGetComponent(out character)) return;

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
    protected bool CanOpen()
    {
        return true;
    }
}
