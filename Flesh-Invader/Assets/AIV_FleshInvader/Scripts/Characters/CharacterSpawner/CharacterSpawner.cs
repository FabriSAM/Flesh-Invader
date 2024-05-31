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
        Character characterToSpawn = Pooler.Instance.GetPooledObject(characterType[Random.Range(0, characterType.Length)]).GetComponent<Character>();
        Vector3 spawnOffset = Random.insideUnitSphere * spawnRadius; 
        spawnOffset.y = 0;
        characterToSpawn.transform.position = PlayerState.Get().transform.position + spawnOffset;
        characterToSpawn.gameObject.SetActive(true);
    }


}
