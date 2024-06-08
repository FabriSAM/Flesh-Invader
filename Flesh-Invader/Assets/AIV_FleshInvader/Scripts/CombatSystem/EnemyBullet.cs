using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour, IBullet
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
    public void Shoot(Transform spawnTransform, float speed)
    {
        transform.position = spawnTransform.position;
        Vector3 velocity = spawnTransform.forward * speed;
        rb.velocity = velocity;
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        InternalTrigger(collision);
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

    public void Shoot(Transform spawnTransform, float speed, IPossessable owner)
    {
        transform.position = spawnTransform.position;
        Vector3 velocity = spawnTransform.forward * speed;
        rb.velocity = velocity;
        this.owner = owner;
        gameObject.SetActive(true);
    }

    protected void InternalTrigger(Collision other)
    {
        IDamageable otherDamageable = other.gameObject.GetComponent<IDamageable>();
        if (otherDamageable == null) return;
        if (other.gameObject == gameObject) return;
        IDamager otherDamager = other.gameObject.GetComponent<IDamager>();
        if (otherDamager == null) return;

        DamageContainer damage = new DamageContainer();
        damage.Damager = otherDamager;
        damage.Damage = owner.CharacterInfo.CharStats.Damage;
        otherDamageable.TakeDamage(damage);
    }
}
