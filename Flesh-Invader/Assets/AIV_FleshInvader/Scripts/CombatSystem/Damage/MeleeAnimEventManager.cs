using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAnimEventManager : MonoBehaviour
{
    [SerializeField]
    private Collider[] colliders;


    private void SetCollidersEnabled()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    private void SetCollidersDisabled()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

}
