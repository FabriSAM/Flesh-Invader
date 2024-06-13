using NotserializableEventManager;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PossessionBullet : MonoBehaviour, IBullet
{
    #region SerializeField
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float lifeTime;
    [Space]
    [Header("CameraEffects")]
    [SerializeField]
    private float defaultFov;
    [SerializeField]
    private float maxFov;
    [SerializeField]
    private float fovSpeed;
    #endregion

    #region PrivateMembers
    private Coroutine lifeCoroutine;
    private IPossessable owner;
    private float speed;
    #endregion

    #region Mono
    private void OnEnable()
    {   
        GlobalEventSystem.AddListener(EventName.PlayerDeath, OnPlayerStateDead);
    }
    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnPlayerStateDead);
    }
    #endregion

    #region PublicMethods
    public void Shoot(Transform spawnTransform, float speed, IPossessable owner)
    {
        transform.position = spawnTransform.position;
        this.speed = speed;
        Vector3 velocity = spawnTransform.forward * speed;
        rb.velocity = velocity;
        this.owner = owner;
        GlobalEventSystem.CastEvent(EventName.CameraFOVChange, EventArgsFactory.CameraFOVChangeFactory(fovSpeed, maxFov, false));
        gameObject.SetActive(true);
        lifeCoroutine = StartCoroutine(LifeCoroutine());
    }

    public void Shoot(Transform spawnTransform, float speed)
    {
    }
    #endregion

    #region PrivateMethods
    private void OnCollisionEnter(Collision collision)
    {
        IPossessable possessableChar = collision.gameObject.GetComponentInChildren<IPossessable>();
        if (possessableChar != null && !possessableChar.UnPossessable)
        {
            owner.UnPossess();
            possessableChar.Possess();
        }
        Destroy();
    }

    private void Destroy()
    {
        GlobalEventSystem.CastEvent(EventName.CameraFOVChange, EventArgsFactory.CameraFOVChangeFactory(fovSpeed, defaultFov, true));
        if(lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
        }
        gameObject.SetActive(false);
    }
    #endregion

    #region Coroutine
    private IEnumerator LifeCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }
    #endregion

    #region CallBacks
    private void OnPlayerStateDead(EventArgs _) 
    {
        Destroy();
    }
    #endregion
}
