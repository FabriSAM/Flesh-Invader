using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


public class SpawnArea : MonoBehaviour
{

    [SerializeField] CharacterSpawner spawner;
    [SerializeField] EnemyPoolProbabilityDict newSpawnStats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            spawner.SpawnProbability = newSpawnStats;
        }
    }


}
