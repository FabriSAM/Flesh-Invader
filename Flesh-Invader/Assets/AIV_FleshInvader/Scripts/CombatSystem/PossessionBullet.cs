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
    [SerializeField]
    private ParticleSystem trail;
    [SerializeField]
    private TrailRenderer trailRenderer;

    #endregion

    #region PrivateMembers
    private Coroutine lifeCoroutine;
    private IPossessable owner;
    private float speed;
    #endregion

    #region FMOD
    private const string possessEventName = "Possess";
    private const string possessEventBank = "Player";
    #endregion

    #region Mono
    private void OnEnable()
    {   
        GlobalEventSystem.AddListener(EventName.PlayerDeath, OnPlayerStateDead);
        trailRenderer.Clear();
        trail.Play();
    }
    private void OnDisable()
    {
        trailRenderer.Clear();
        GlobalEventSystem.RemoveListener(EventName.PlayerDeath, OnPlayerStateDead);
    }
    private void OnTriggerEnter(Collider other)
    {
        IPossessable possessableChar = other.gameObject.GetComponentInChildren<IPossessable>();
        if (possessableChar != null && !possessableChar.UnPossessable && !possessableChar.IsDead)
        {
            owner.UnPossess();
            possessableChar.Possess();
            AudioManager.Get().PlayOneShot(possessEventName, possessEventBank);
        }
        Destroy();
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

    private void Destroy()
    {
        GlobalEventSystem.CastEvent(EventName.CameraFOVChange, EventArgsFactory.CameraFOVChangeFactory(fovSpeed, defaultFov, true));
        if(lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
        }
        trail.Stop();
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
