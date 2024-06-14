using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibleBuildings : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;

    private List<GameObject> buildings = new List<GameObject>();
    void FixedUpdate()
    {
        RaycastHit[] hits = Physics.RaycastAll(PlayerState.Get().CurrentPlayer.transform.position,
             gameObject.transform.position - PlayerState.Get().CurrentPlayer.transform.position, (gameObject.transform.position - PlayerState.Get().CurrentPlayer.transform.position).sqrMagnitude, LayerMask.GetMask("StaticEnv"));
        if (hits.Length == 0 || CheckContain(hits)) ClearArray();

        foreach (RaycastHit hit in hits)
        {
            hit.transform.gameObject.GetComponentInChildren<MeshRenderer>().material = materials[1];
            buildings.Add(hit.transform.gameObject);
        }
    }
    private void ClearArray()
    {
        for (int i = buildings.Count - 1; i >= 0; i--)
        {
            buildings[i].GetComponentInChildren<MeshRenderer>().material = materials[0];
            buildings.RemoveAt(i);
        }
    }

    private bool CheckContain(RaycastHit[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            if (!buildings.Contains(hits[i].transform.gameObject))
            {
                return false;
            }
        }

        return true;
    }
}
