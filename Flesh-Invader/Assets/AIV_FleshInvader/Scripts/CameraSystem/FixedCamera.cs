using NotserializableEventManager;
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
    #endregion

    #region SerializeField
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private Camera ownerCamera;
    [SerializeField]
    Vector3 offset;
    #endregion

    #region Private Members
    private Vector3 newPos;
    private Vector3 currentPos;
    private Coroutine cameraShakeCoroutine;
    private Coroutine cameraFovCoroutine;
    #endregion

    #region Mono
    private void OnEnable()
    {
        GlobalEventSystem.AddListener(EventName.CameraFOVChange, CameraFovCallBack);
        GlobalEventSystem.AddListener(EventName.CameraShake, CameraShakeCallBack);
    }
    void Start()
    {
        newPos = gameObject.transform.position;
        currentPos = gameObject.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }
    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.CameraFOVChange, CameraFovCallBack);
        GlobalEventSystem.RemoveListener(EventName.CameraShake, CameraShakeCallBack);
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
        float startFov = ownerCamera.fieldOfView;
        endFov=Mathf.Clamp(endFov,minFov,maxFov);
        if (startFov > endFov)
        {
            while (ownerCamera.fieldOfView!=endFov)
            {
                ownerCamera.fieldOfView -= speed * Time.deltaTime;
                ownerCamera.fieldOfView=Mathf.Clamp(ownerCamera.fieldOfView,endFov,startFov);
                yield return new WaitForEndOfFrame();
            }  
        }
        if(startFov < endFov)
        {
            while (ownerCamera.fieldOfView != endFov)
            {
                ownerCamera.fieldOfView += speed * Time.deltaTime;
                ownerCamera.fieldOfView = Mathf.Clamp(ownerCamera.fieldOfView, startFov, endFov);
                yield return new WaitForEndOfFrame();
            }
        }
        cameraFovCoroutine = null;
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region CallBacks
    private void CameraFovCallBack(EventArgs message)
    {
        EventArgsFactory.CameraFOVParser(message, out float speed, out float newFov, out bool overrideCoroutine);
        CameraFov(speed,newFov,overrideCoroutine);
    }

    private void CameraShakeCallBack(EventArgs message) 
    {
        EventArgsFactory.CameraShakeParser(message, out float amplitude, out float duration, out bool overrideCoroutine);
        CameraShake(amplitude,duration,overrideCoroutine);
    }
    #endregion
}
