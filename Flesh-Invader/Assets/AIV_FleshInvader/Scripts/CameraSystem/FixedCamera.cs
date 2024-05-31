using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FixedCamera : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float anchorDistance;
    [SerializeField]
    private bool outOfRange;
    #region Private Members
    private Vector3 offset;
    private Vector3 newPos;
    private Vector3 currentPos;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        offset = gameObject.transform.position - PlayerState.Get().PlayerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }

    private void LerpCameraMovement()
    {
        gameObject.transform.position = Vector3.Lerp(currentPos, newPos, cameraSpeed * Time.deltaTime);
    }
    private void AnchorCameraMovement()
    {
        gameObject.transform.position = newPos;
    }

    private void CameraMovement()
    {
        newPos = PlayerState.Get().PlayerTransform.position + offset;
        currentPos = gameObject.transform.position;
        AnchorCameraMovement();
        //float sqrDistance = Vector3.SqrMagnitude(newPos - currentPos);
        //Debug.Log(sqrDistance);
        //if (sqrDistance >= anchorDistance * anchorDistance)
        //{
        //    outOfRange = true;
        //}
        //if(outOfRange)
        //{
        //    LerpCameraMovement();
        //    if (sqrDistance <= 0.02)
        //    {
        //        outOfRange=false;
        //    }
        //    return;
        //}
        //AnchorCameraMovement();
        //if (sqrDistance <= anchorDistance*anchorDistance)
        //{
        //    AnchorCameraMovement();

        //    return;
        //}
        //LerpCameraMovement();
    }
}
