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
    private Coroutine cameraShakeCoroutine;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        offset = gameObject.transform.position - PlayerState.Get().CurrentPlayer.transform.position;
        newPos = gameObject.transform.position;
        currentPos = gameObject.transform.position;
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
    private void CameraMovement()
    {
        newPos = PlayerState.Get().CurrentPlayer.transform.position + offset;
        currentPos = gameObject.transform.position;
        LerpCameraMovement();
    }
    public void CameraShake(float amplitude, float duration)
    {
        cameraShakeCoroutine = StartCoroutine(CameraShakeCoroutine(amplitude,duration));
    }

    IEnumerator CameraShakeCoroutine(float amplitude, float duration)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            gameObject.transform.position+=Random.insideUnitSphere*amplitude*Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
