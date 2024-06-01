using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : InteractableBase
{
    #region SerializedField
    [SerializeField]
    protected GameObject gate;
    #endregion

    #region Variables
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

    #region OverrideBaseClass
    protected override void InternalOnTriggerEnter(Collider other, bool status)
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
    protected override void OnOpen()
    {
        open = StartCoroutine(OpenDoor());
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
    #endregion

}
