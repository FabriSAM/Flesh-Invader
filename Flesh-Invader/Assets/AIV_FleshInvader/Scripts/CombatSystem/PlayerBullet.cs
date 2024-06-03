using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet: MonoBehaviour, IBullet
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float lifeTime;

    private Coroutine lifeCoroutine;

    private void OnEnable()
    {
        lifeCoroutine = StartCoroutine(LifeCoroutine());
    }
    public void Shoot(Transform spawnTransform, float speed)
    {
        gameObject.SetActive(true);
        transform.position = spawnTransform.position;
        Vector3 velocity = spawnTransform.forward * speed;
        rb.velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
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
}
