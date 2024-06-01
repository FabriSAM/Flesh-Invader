using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour, IPoolRequester
{
    [SerializeField] private PoolData[] characterType;

    [SerializeField] protected float spawnRadius;
    [SerializeField] protected float spawnTime;
    [SerializeField] protected float spawnTimeCounter;


    public PoolData[] Datas
    {
        get { return characterType; }
    }

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
        PoolData pool = characterType[Random.Range(0, characterType.Length)];
        GameObject obj =  Pooler.Instance.GetPooledObject(pool);
        EnemyChar characterToSpawn = obj.GetComponentInChildren<EnemyChar>();

        if (characterToSpawn != null)
        {
            Vector2 spawnOffset2D = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnOffset = new Vector3(spawnOffset2D.x,0,spawnOffset2D.y);
            obj.transform.position = PlayerState.Get().PlayerTransform.position + spawnOffset;
            obj.SetActive(true);
        }
    }


}
