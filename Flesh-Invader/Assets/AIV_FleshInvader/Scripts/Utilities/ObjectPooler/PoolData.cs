using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PoolData", menuName = "ObjectPooling/Pool", order = 1)]
public class PoolData : ScriptableObject
{

    [SerializeField]
    private string poolKey;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int poolNumber;
    [Tooltip("Qui ci mettete tutto quello che volete")]
    [SerializeField]
    private bool resizablePool;

    public string PoolKey {
        get { return poolKey; }
    }
    public GameObject Prefab {
        get { return prefab; }
    }
    public int PoolNumber {
        get { return poolNumber; }
    }
    public bool ResizablePool {
        get { return resizablePool; }
    }

}
