using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    #region Private Members
    private Vector3 Offset;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Offset = gameObject.transform.position - PlayerState.Get().PlayerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = PlayerState.Get().PlayerTransform.position + Offset;
    }
}
