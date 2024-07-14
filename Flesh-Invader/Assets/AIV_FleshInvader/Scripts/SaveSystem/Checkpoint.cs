using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Vector3 spawnPositionOffset;
    private float offsetSphereRadius = 3;

    private void OnTriggerEnter(Collider other)
    {
        SaveSystem.SaveGameStats(transform.position + spawnPositionOffset);
        other.gameObject.GetComponent<Controller>().VFXMng.ActivateEffect("Save");
        gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        SaveSystem.SaveGameStats(transform.position + spawnPositionOffset);
        gameObject.SetActive(false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + spawnPositionOffset, offsetSphereRadius);
    }

 

}
