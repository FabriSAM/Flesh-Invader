using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    GameObject gate;
    [SerializeField]
    Collider gateCollider;
    [SerializeField]
    GameObject canvas;

    private Controller controller;

    private void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        InternalOnTriggerEnter(other);
    }
    private void InternalOnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);

        if (other.TryGetComponent(out controller))
        {
            //Sottoscrivo L'evento
        }
    }
    private void OnOpen()
    {
        StartCoroutine(OpenDoor());
    }


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
        
        
    }

}
