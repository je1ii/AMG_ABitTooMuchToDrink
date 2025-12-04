using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<EnemyData> availableEnemies = new List<EnemyData>();
    [SerializeField] private GameObject playerTarget;
    [SerializeField] private Transform parent;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnBaseCartesianPosition = new Vector3(30f, 30f, 0f);
    [SerializeField] private float spawnScatterRadius = 2f;
    [SerializeField] private int spawnCap = 10;
    [SerializeField] private float defaultInterval = 3f;
    [SerializeField] private float dizzyInterval = 2f;
    [SerializeField] private float drunkInterval = 1f;
    
    private Coroutine spawnCoroutine = null; 
    private float totalWeight;
    
    private PlayerMovement pm;
    
    void Start()
    {
        if (playerTarget == null)
        {
            Debug.LogError("Player prefab is not assigned.");
        }
        
        CalculateTotalWeight();
        pm = playerTarget.GetComponent<PlayerMovement>();
        
        if (pm != null && spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnEnemyLoop());
        }
    }

    void Update()
    {
        SetWeightWithState();
        int currentEnemies = PlayerThrow.GetEnemiesCount();

        if (currentEnemies >= spawnCap)
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }
        else
        {
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnEnemyLoop());
            }
        }
    }

    private void CalculateTotalWeight()
    {
        totalWeight = 0f;
        foreach (var data in availableEnemies)
        {
            totalWeight += data.spawnWeight;
        }
        if (totalWeight <= 0)
        {
            Debug.LogError("Total spawn weight is zero or negative. Check EnemyData settings.");
        }
    }
    
    private EnemyData GetWeightedRandomEnemy()
    {
        if (availableEnemies.Count == 0 || totalWeight <= 0) return null;

        float randomWeight = Random.Range(0f, totalWeight);
        float currentWeightSum = 0f;

        foreach (var data in availableEnemies)
        {
            currentWeightSum += data.spawnWeight;
            
            if (randomWeight <= currentWeightSum)
            {
                return data;
            }
        }
        
        return availableEnemies[availableEnemies.Count - 1]; 
    }
    
    private IEnumerator SpawnEnemyLoop()
    {
        while(true)
        {
            EnemyData enemyToSpawn = GetWeightedRandomEnemy();

            if (enemyToSpawn == null || enemyToSpawn.prefab == null || playerTarget == null)
            {
                yield return new WaitForSeconds(defaultInterval); 
                continue;
            }

            Vector3 scatter = new Vector3(
                Random.Range(-spawnScatterRadius, spawnScatterRadius),
                Random.Range(-spawnScatterRadius, spawnScatterRadius),
                0f
            );
            
            Vector3 finalSpawnCartesianPos = spawnBaseCartesianPosition + scatter;
        
            Vector3 initialVisualPos = Utils.CartesianToIsometric(finalSpawnCartesianPos);

            GameObject enemyObj = Instantiate(
                enemyToSpawn.prefab,
                initialVisualPos,
                Quaternion.identity,
                parent
            );
        
            EnemyMovement movement = enemyObj.GetComponent<EnemyMovement>();
            EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();

            if (movement != null)
            {
                movement.AssignSpeed(enemyToSpawn.speed);
                movement.AssignDamage(enemyToSpawn.damage);
                movement.AssignSprites(enemyToSpawn.sprites);
                movement.Initialize(finalSpawnCartesianPos, playerTarget.transform);
            }
            if (health != null)
            {
                health.AssignHealth(enemyToSpawn.health);
                health.AssignDrops(enemyToSpawn.drops);
                health.AssignParent(parent);
            }
        
            Debug.Log($"Spawned: {enemyToSpawn.enemyID}");
            
            yield return new WaitForSeconds(GetCorrectInterval());
        }
    }

    private float GetCorrectInterval()
    {
        if (pm == null) return defaultInterval;
        
        if (pm.isDrunk)
            return drunkInterval;
        if (pm.isDizzy)
            return dizzyInterval;
        
        return defaultInterval;
    }
    
    private void SetWeightWithState()
    {
        if (pm.isDrunk)
            AdjustSpawnWeight("Green", 5f);
        if (pm.isDizzy)
            AdjustSpawnWeight("Green", 2f);
        else
            AdjustSpawnWeight("Green", 0f);
    }

    private void AdjustSpawnWeight(string enemyID, float newWeight)
    {
        EnemyData data = availableEnemies.Find(e => e.enemyID == enemyID);
        if (data != null)
        {
            data.spawnWeight = newWeight;
            CalculateTotalWeight();
        }
    }
}
