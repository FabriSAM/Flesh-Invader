using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatisticsTemplate",
    menuName = "EnemyTemplate/EnemyStatisticsTemplate", order = 1)]
public class EnemyStatisticsTemplate : ScriptableObject
{
    [SerializeField]
    private EnemyType enemyType;
    [Header("Stats")]
    [SerializeField]
    private float health;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float xp;
    [Header("Narrative")]
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private string passiveAbilityDescription;
    [SerializeField]
    private string baseAbilityDescription;

    public EnemyType EnemyType { get { return enemyType; } }
    public Sprite Icon { get { return icon; } }
    public string PassiveAbilityDescription { get { return passiveAbilityDescription; } }
    public string BaseAbilityDescription { get { return baseAbilityDescription; } }
    public float Health { get { return health; } }
    public float Damage { get { return damage; } }
    public float Speed { get { return speed; } }
    public float XP { get { return xp; } }

    private void OnDestroy()
    {
       
    }
}
