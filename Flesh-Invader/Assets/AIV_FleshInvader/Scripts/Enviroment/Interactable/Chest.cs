using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    Collider trigger;
    [SerializeField]
    GameObject canvas;
    #endregion

    #region Variables
    private Controller controller;
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

    #region InternalMethods
    private void InternalOnTriggerEnter(Collider other, bool status)
    {
        if (other.TryGetComponent(out controller))
        {
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
    }
    #endregion

    #region PrivateMethods
    private void SubscribeInteract()
    {
        controller.OnInteractPerformed += OnOpen;
    }
    private void UnscribeInteract()
    {
        controller.OnInteractPerformed -= OnOpen;
    }
    void OnOpen()
    {
        //TO DO
    }
    #endregion
}
