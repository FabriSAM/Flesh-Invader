using NotserializableEventManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

[Serializable]
public struct EnemyPoolProbabilityDict
{
    [SerializeField] public ObjectByProbability<string>[] poolProbability;
}

public class CharacterSpawner : MonoBehaviour, IPoolRequester
{
    // Hipotetically we could use a special PoolData array to bring some special enemyPool that have to spawn independently from zone
    //[SerializeField] private PoolData[] specialCharacterType
    //[SerializeField] private EnemyPoolProbabilityDict specialSpawnProbability;

    [SerializeField] private PoolData[] characterType;
    [SerializeField] private int maxEnemiesInScene;

    [Tooltip("Enemy distance from player at spawn")]
    [SerializeField] protected float spawnRadius;
    [SerializeField] protected float spawnTime;
    protected float spawnTimeCounter;

    [SerializeField] private EnemyPoolProbabilityDict spawnProbability;
    private NavMeshHit navMeshSpawnHit;

    private int activeEnemies;

    public PoolData[] Datas
    {
        get { return characterType; }
    }
    public float SpawnRadius { get { return spawnRadius; } set { spawnRadius = value; } }
    public float SpawnTime { get { return SpawnTime; } set { SpawnTime = value; } }

    public EnemyPoolProbabilityDict SpawnProbability { 
        get => spawnProbability; 
        set => spawnProbability = value; }


    private void Awake()
    {
        GlobalEventSystem.AddListener(EventName.EnemyDeath, CountEnemyDeath);
    }

    private void Update()
    {
        spawnTimeCounter += Time.deltaTime;

        if (spawnTimeCounter >= spawnTime && activeEnemies < maxEnemiesInScene)
        {
            spawnTimeCounter = 0;
            SpawnCharacter();
        }
    }
    private string SelectEnemyPoolByProbability()
    {
        float probabilityValue = UnityEngine.Random.Range(0f, 1f);
        foreach (ObjectByProbability<string> item in SpawnProbability.poolProbability)
        {

            if(item.IsInRange(probabilityValue))
            {
                //Debug.Log("Probability: " + probabilityValue);

                return item.result;
            }
        }

        return "Probability Not Found";
    }

    private void SpawnCharacter()
    {
        string enemyPoolName = SelectEnemyPoolByProbability();
        
        PoolData pool = Array.Find(characterType, pool => { return pool.PoolKey == enemyPoolName; });
        GameObject obj =  Pooler.Instance.GetPooledObject(pool);
        EnemyChar characterToSpawn = obj.GetComponentInChildren<EnemyChar>();

        if (characterToSpawn != null)
        {
            Vector2 spawnOffset2D = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 spawnOffset = new Vector3(spawnOffset2D.x,0,spawnOffset2D.y);

            NavMesh.SamplePosition(
                PlayerState.Get().CurrentPlayer.transform.position + spawnOffset,
                out navMeshSpawnHit, 2000, 1
            ) ;

            obj.transform.position = navMeshSpawnHit.position;

            obj.SetActive(true);
            activeEnemies++;
        }
    }

    private void CountEnemyDeath(EventArgs _)
    {
        activeEnemies--;
    }

}
