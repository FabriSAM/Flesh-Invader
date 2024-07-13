using NotserializableEventManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

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

    private const string bossPoolKey = "Boss";
    private const string roguePoolKey = "Rogue";

    private static CharacterSpawner instance;
    
    public static CharacterSpawner GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<CharacterSpawner>();
        }

        return instance;
    }

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

    private void OnDisable()
    {
        GlobalEventSystem.RemoveListener(EventName.EnemyDeath, CountEnemyDeath);
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
            characterToSpawn.CalculateUnpossessability();

            obj.SetActive(true);
            activeEnemies++;
        }
    }

    // I didn't use the entire class PlayerSavedData beacause i would have to create a circular dependency between assemblies
    public void LoadPlayerCharacter(float playerMaxHealth, float playerHealth, Vector3 lastPosition, Statistics playerStats, LevelStruct playerLevel, EnemyInfo enemyInfo)
    {
        GameObject playerChar;
        PoolData pool;

        // Player Spawning
        switch (enemyInfo.CharStats.EnemyType)
        {
            case EnemyType.Boss:
                pool = Array.Find(characterType, pool => { return pool.PoolKey == bossPoolKey; });              
                playerChar = (Instantiate(pool.Prefab, lastPosition, Quaternion.identity));
                break;
            case EnemyType.Thief:
                pool = Array.Find(characterType, pool => { return pool.PoolKey == roguePoolKey; });
                playerChar = (Instantiate(pool.Prefab, lastPosition, Quaternion.identity));
                break;
            default:
                playerChar = null;
                break;
        }

        if (playerChar != null)
        {
            // Move to Checkpoint
            playerChar.GetComponentInChildren<NavMeshAgent>().enabled = false;

            Controller playerController = playerChar.GetComponentInChildren<Controller>();
            EnemyChar playerAsCharacter = playerChar.GetComponentInChildren<EnemyChar>();

            PlayerInitialPossessionOnLoad(playerChar, playerAsCharacter, playerController);

            PlayerInitialSetUpOnLoad(playerMaxHealth, playerHealth, lastPosition, playerStats, playerLevel, enemyInfo, playerChar, playerAsCharacter);

            GlobalEventSystem.CastEvent(EventName.PossessionExecuted, EventArgsFactory.PossessionExecutedFactory(enemyInfo));

        }
    }


    private static void PlayerInitialPossessionOnLoad(GameObject playerChar, EnemyChar playerAsChar, Controller playerController)
    {

        PlayerState.Get().CurrentPlayer.GetComponentInChildren<Controller>().UnPossess();
        PlayerState.Get().CurrentPlayer.gameObject.SetActive(false);
        // Possession behavior
        playerChar.gameObject.SetActive(true);
        

        playerAsChar.stateMachine.enabled = false;
        playerController.IsPossessed = true;
    }
    private static void PlayerInitialSetUpOnLoad(float playerMaxHealth, float playerHealth, Vector3 lastPosition, Statistics playerStats, LevelStruct playerLevel, EnemyInfo enemyInfo, GameObject playerChar, EnemyChar playerAsCharacter)
    {
        // Set Character stats
        playerAsCharacter.CharacterInfo = enemyInfo;
        // Set Player Level
        PlayerState.Get().LevelController.SetLevel(playerLevel);
        // Set Player Stats
        PlayerState.Get().InformationController.SetStats(playerStats);
        // Set Player MaxLife
        PlayerState.Get().HealthController.MaxHealthSet(playerMaxHealth);
        // Set Player Life
        PlayerState.Get().HealthController.HealthSet(playerHealth);

        // Why It did not work?
        //playerChar.transform.position = lastPosition;
    }

    private void CountEnemyDeath(EventArgs _)
    {
        activeEnemies--;
    }

}
