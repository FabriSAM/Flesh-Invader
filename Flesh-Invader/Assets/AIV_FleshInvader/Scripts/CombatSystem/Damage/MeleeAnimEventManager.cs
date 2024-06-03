using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimEventManager
{
    private void SetColliderEnabled(object InObject)
    {
        Collider collider = ((GameObject)InObject).GetComponent<Collider>();
        if (collider == null) return;
        collider.enabled = true;
    }

    private void SetColliderDisabled(object InObject)
    {
        Collider collider = ((GameObject)InObject).GetComponent<Collider>();
        if (collider == null) return;
        collider.enabled = false;
    }
}
