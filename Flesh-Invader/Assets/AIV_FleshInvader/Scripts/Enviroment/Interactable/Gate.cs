using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    GameObject gate;
    [SerializeField]
    Collider trigger;
    [SerializeField]
    GameObject canvas;
    #endregion

    #region Variables
    private Controller controller;
    private Coroutine open;
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
    private void OnOpen()
    {
        open = StartCoroutine(OpenDoor());
    }
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
    #endregion

}
