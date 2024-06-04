using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PossessionBullet : MonoBehaviour, IBullet
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float lifeTime;

    private Coroutine lifeCoroutine;
    private IPossessable owner;

    private void OnEnable()
    {        
        lifeCoroutine = StartCoroutine(LifeCoroutine());
    }
    public void Shoot(Transform spawnTransform, float speed, IPossessable owner)
    {
        transform.position = spawnTransform.position;
        Vector3 velocity = spawnTransform.forward * speed;
        rb.velocity = velocity;
        this.owner = owner;
        gameObject.SetActive(true);
    }

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
        gameObject.SetActive(false);
    }

    private IEnumerator LifeCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }

    public void Shoot(Transform spawnTransform, float speed)
    {
    }
}