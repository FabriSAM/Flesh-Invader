public enum DamageType
{
    Melee,
    Ranged
}

public interface IDamageable
{

    void TakeDamage(DamageContainer damage);

}
