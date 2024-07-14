using System;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    [SerializeField]
    private bool attackOnStay;

    public Action<IDamageable, IPossessable, Vector3> DamageableHitted;

    protected Collider myCollider;
    public Collider MyCollider
    {
        get 
        {
            if (myCollider == null) { myCollider = GetComponent<Collider>(); }
            return myCollider;
        }
    }


    protected void OnTriggerEnter(Collider other)
    {
        InternalTrigger(other);
    }

    protected void OnTriggerStay(Collider other)
    {
        if (!attackOnStay) return;
        InternalTrigger(other);
    }

    protected void InternalTrigger(Collider other)
    {
        IPossessable owner = gameObject.GetComponentInParent<IPossessable>();
        if (owner == null) return;

        IDamageable otherDamageable = other.GetComponent<IDamageable>();
        if (otherDamageable == null) return;
        if (other.gameObject == gameObject) return;
        Vector3 hitPosition = other.ClosestPoint(transform.position);

        // The collision itself doesn't need checks because it is already computed based on Layers in Controller
        DamageableHitted?.Invoke(otherDamageable, owner, hitPosition);
    }
}
