using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct EnemyPoolProbabilityDict
{
    [SerializeField] public ObjectByProbability<string>[] poolProbability;
}

public class CharacterSpawner : MonoBehaviour, IPoolRequester
{
    [SerializeField] private PoolData[] characterType;

    [SerializeField] protected float spawnRadius;
    [SerializeField] protected float spawnTime;
    [SerializeField] protected float spawnTimeCounter;

    [SerializeField] protected EnemyPoolProbabilityDict spawnProbability;


    public PoolData[] Datas
    {
        get { return characterType; }
    }
    public float SpawnRadius { get { return spawnRadius; } set { spawnRadius = value; } }
    public float SpawnTime { get { return SpawnTime; } set { SpawnTime = value; } }


    private void Update()
    {
        spawnTimeCounter += Time.deltaTime;

        if (spawnTimeCounter >= spawnTime)
        {
            spawnTimeCounter = 0;
            SpawnCharacter();
        }
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
            obj.transform.position = PlayerState.Get().PlayerTransform.position + spawnOffset;
            obj.SetActive(true);
        }
    }

    private string SelectEnemyPoolByProbability()
    {
        float probabilityValue = UnityEngine.Random.Range(0f, 1f);
        foreach (ObjectByProbability<string> item in spawnProbability.poolProbability)
        {

            if(item.IsInRange(probabilityValue))
            {
                Debug.Log("Probability: " + probabilityValue);

                return item.result;
            }
        }

        return "Probability Not Found";
    }
}
