using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class FixedCamera : MonoBehaviour
{
    #region Consts
    private const float minFov = 10.0f;
    private const float maxFov = 120.0f;
    private const float defaultFovSpeed = 50.0f;
    #endregion

    #region SerializeField
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private Camera ownerCamera;
    #endregion

    #region Private Members
    private Vector3 offset;
    private Vector3 newPos;
    private Vector3 currentPos;
    private float defaultFov;
    private Coroutine cameraShakeCoroutine;
    private Coroutine cameraFovCoroutine;
    #endregion

    #region Mono
    void Start()
    {
        offset = gameObject.transform.position - PlayerState.Get().CurrentPlayer.transform.position;
        newPos = gameObject.transform.position;
        currentPos = gameObject.transform.position;
        defaultFov = ownerCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }
    #endregion

    #region privateMethods
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
    private void CameraShake(float amplitude, float duration,bool overrideCoroutine)
    {
        if (overrideCoroutine && cameraShakeCoroutine != null)
        {
            StopCoroutine(cameraShakeCoroutine);
            cameraShakeCoroutine = null;
        }
        if (cameraShakeCoroutine != null) return;
        cameraShakeCoroutine = StartCoroutine(CameraShakeCoroutine(amplitude,duration));
    }
    private void CameraFov(float speed, float endFov, bool overrideCoroutine)
    {
        if (overrideCoroutine && cameraFovCoroutine != null) 
        {
            StopCoroutine(cameraFovCoroutine);
            cameraFovCoroutine = null;
        }
        if(cameraFovCoroutine != null) return;
        endFov = Mathf.Clamp(endFov, minFov, maxFov);
        cameraFovCoroutine=StartCoroutine(CameraFovCoroutine(speed,endFov));
    }
    private void ReturnToDefaultFov()
    {
        CameraFov(defaultFovSpeed, defaultFov, true);
    }
    #endregion

    #region Coroutine
    IEnumerator CameraShakeCoroutine(float amplitude, float duration)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            gameObject.transform.position+=Random.insideUnitSphere*amplitude*Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cameraShakeCoroutine = null;
        yield return new WaitForEndOfFrame();
    }

    IEnumerator CameraFovCoroutine(float speed, float endFov)
    {
        if (ownerCamera.fieldOfView == endFov) yield break;

        if (ownerCamera.fieldOfView > endFov)
        {
            speed *= -1;
        }
        while (ownerCamera.fieldOfView - endFov <= 0.1)
        {
            ownerCamera.fieldOfView += speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cameraFovCoroutine = null;
        yield return new WaitForEndOfFrame();
    }
    #endregion
}
