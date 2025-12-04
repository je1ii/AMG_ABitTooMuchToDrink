using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerSpawner : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject beerPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject player;
    
    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnBaseCartesianPosition = new Vector3(30f, 30f, 0f);
    [SerializeField] private float spawnScatterRadius = 2f;
    [SerializeField] private int spawnCap = 20;
    [SerializeField] private float defaultInterval = 3f;
    [SerializeField] private float dizzyInterval = 3.5f;
    [SerializeField] private float drunkInterval = 4f;

    private Coroutine spawnCoroutine = null; 
    
    PlayerMovement pm;
    
    void Start()
    {
        if (player == null)
            Debug.LogError("Player prefab is not assigned.");
        
        pm = player.GetComponent<PlayerMovement>();

        if (pm != null && spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnBeerLoop());
        }
    }

    void Update()
    {
        List<GameObject> existingBeers = PlayerInteract.GetAllBeers();
        int currentBeers = existingBeers != null ? existingBeers.Count : 0;
        
        if (currentBeers >= spawnCap)
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
                spawnCoroutine = StartCoroutine(SpawnBeerLoop());
            }
        }
    }
    
    private IEnumerator SpawnBeerLoop()
    {
        while(true)
        {
            if (PlayerInteract.GetAllBeers().Count >= spawnCap)
            {
                yield break; 
            }
            
            if (beerPrefab == null) yield break;

            Vector3 scatter = new Vector3(
                Random.Range(-spawnScatterRadius, spawnScatterRadius),
                Random.Range(-spawnScatterRadius, spawnScatterRadius),
                0f
            );
            
            Vector3 finalSpawnCartesianPos = spawnBaseCartesianPosition + scatter;
        
            Vector3 initialVisualPos = Utils.CartesianToIsometric(finalSpawnCartesianPos);

            Instantiate(
                beerPrefab,
                initialVisualPos,
                Quaternion.identity,
                parent
            );
            
            Debug.Log($"Spawned: {beerPrefab.name}");
            
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
}
