using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibleBuildings : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;

    private Dictionary<GameObject, Material> buildings = new Dictionary<GameObject, Material>();
    void FixedUpdate()
    {
        RaycastHit[] hits = Physics.RaycastAll(PlayerState.Get().CurrentPlayer.transform.position,
             gameObject.transform.position - PlayerState.Get().CurrentPlayer.transform.position, (gameObject.transform.position - PlayerState.Get().CurrentPlayer.transform.position).sqrMagnitude, LayerMask.GetMask("StaticEnv"));
        if (hits.Length == 0 || CheckContain(hits)) ClearDictionary();

        foreach (RaycastHit hit in hits)
        {
            if (buildings.ContainsKey(hit.transform.gameObject) || 
                hit.transform.gameObject.GetComponentInChildren<MeshRenderer>().material == materials[0]) continue;
            buildings.Add(hit.transform.gameObject, hit.transform.gameObject.GetComponentInChildren<MeshRenderer>().material);
            hit.transform.gameObject.GetComponentInChildren<MeshRenderer>().material = materials[0];
        }
    }
    private void ClearDictionary()
    {
        for (int i = buildings.Count - 1; i >= 0; i--)
        {
            buildings.ElementAt(i).Key.GetComponentInChildren<MeshRenderer>().material = buildings.ElementAt(i).Value;
            buildings.Remove(buildings.ElementAt(i).Key);
        }
    }

    private bool CheckContain(RaycastHit[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            if (!buildings.ContainsKey(hits[i].transform.gameObject))
            {
                return false;
            }
        }

        return true;
    }
}
