using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CombatManager : MonoBehaviour, IDamageable, IDamager
{
    #region Serialized Fields
    [SerializeField]
    protected MeleeCollider[] meleeColliders;
    [SerializeField]
    protected HealthModule healthModule;
    #endregion

    #region Getters
    public HealthModule HealthModule
    {
        get { return healthModule; }
    }
    #endregion

    #region Actions
    public Action<float> OnControllerEnabled;
    public Action<string> OnPossessionChanged;

    public Action<DamageContainer> OnPerceivedDamage;
    public Action<DamageContainer> OnControllerDamageTaken;
    public Action<DamageContainer> OnHealthModuleDamageTaken;
    public Action OnHealthModuleDeath;
    #endregion

    #region Mono
    private void Awake()
    {
        OnControllerEnabled += OnInternalControllerEnabled;
        OnPossessionChanged += OnInternalPossessionChanged;
        OnControllerDamageTaken += OnInternalDamageTaken;

        healthModule.OnDamageTaken += OnHealthModuleDamageNotify;
        healthModule.OnDeath += OnHealthModuleDeathNotify;

        foreach (MeleeCollider collider in meleeColliders)
        {
            collider.DamageableHitted += OnMeleeHitted;
        }
    }
    #endregion

    #region Callbacks
    private void OnMeleeHitted(IDamageable otherDamageable, IPossessable owner, Vector3 hitPosition)
    {
        DamageContainer damage = new DamageContainer();
        damage.Damage = owner.CharacterInfo.CharStats.Damage;
        damage.Damager = this;

        otherDamageable.TakeDamage(damage);
    }

    private void OnInternalPossessionChanged(string obj)
    {
        SetDamagerCollidersLayerType(obj);
    }
    #endregion

    #region Private Methods
    private void SetDamagerCollidersLayerType(string newLayer)
    {
        foreach (MeleeCollider collider in meleeColliders)
        {
            Collider currentCollider = collider.GetComponent<Collider>();
            if (currentCollider == null) continue;
            currentCollider.gameObject.layer = LayerMask.NameToLayer(newLayer);
        }
    }
    #endregion

    #region Interface Methods
    public void TakeDamage(DamageContainer damage)
    {
        OnPerceivedDamage?.Invoke(damage);
    }
    #endregion

    #region Health Module
    private void OnInternalControllerEnabled(float newHP)
    {
        healthModule.SetHP(newHP);
    }
    private void OnInternalDamageTaken(DamageContainer damage)
    {
        Debug.Log($"Current HP: {healthModule.CurrentHP} - MaxHP : {healthModule.MaxHP}");
        healthModule.TakeDamage(damage);
    }
    private void OnHealthModuleDeathNotify()
    {
        OnHealthModuleDeath?.Invoke();
    }
    private void OnHealthModuleDamageNotify(DamageContainer damage)
    {
        OnHealthModuleDamageTaken?.Invoke(damage);
    }
    #endregion
}
