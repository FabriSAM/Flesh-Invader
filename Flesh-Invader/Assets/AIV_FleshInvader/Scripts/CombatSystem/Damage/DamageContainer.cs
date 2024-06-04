using UnityEngine;
using System;

[Serializable]
public class DamageContainer
{

    #region SerializeField
    [SerializeField]
    private IDamager damager;
    [SerializeField]
    private DamageType damageType;
    [SerializeField]
    private float damage;
    [SerializeField]
    private Vector3 damageImpulse;
    [SerializeField]
    private float freezeTime;
    #endregion

    private Vector3 contactPoint;

    #region PublicProperties
    public IDamager Damager
    {
        get { return damager; }
        set { damager = value; }

    }

    public DamageType DamageType
    {
        get { return damageType; }
    }

    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public Vector3 DamageImpulse
    {
        get { return damageImpulse; }
    }

    public float FreezeTime
    {
        get { return freezeTime; }
    }

    public Vector3 ContactPoint
    {
        get { return contactPoint; }
        set { contactPoint = value; }
    }

    public bool DamageResult
    {
        get;
        set;
    }

    public Vector3 AttackImpulse
    {
        get;
        set;
    }
    #endregion

    #region PublicMethods
    public void SetContactPoint(Vector3 contactPoint)
    {
        this.contactPoint = contactPoint;
    }
    #endregion

}
