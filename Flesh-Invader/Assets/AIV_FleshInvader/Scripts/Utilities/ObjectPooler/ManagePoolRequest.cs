using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePoolRequeste : MonoBehaviour
{

    private void Awake() {
        IPoolRequester[] requesters = GetComponentsInChildren<IPoolRequester>(true);
        foreach(IPoolRequester requester in requesters) {
            foreach(PoolData data in requester.Datas) {
                Pooler.Instance.AddToPool(data);
            }
        }
    }

}
