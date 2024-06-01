using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        IPossessable possessableChar = collision.gameObject.GetComponentInChildren<IPossessable>();
        if (possessableChar != null)
        {
            possessableChar.Possess();
        }
        // Repooling

    }
}
