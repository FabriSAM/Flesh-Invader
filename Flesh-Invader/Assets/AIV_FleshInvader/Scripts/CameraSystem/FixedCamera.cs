using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FixedCamera : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;

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
        LerpCameraMovement();
    }
}
