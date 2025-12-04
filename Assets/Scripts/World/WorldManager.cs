using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private List<TerrainChunk> terrainChunkPrefabs;

    [Header("Generation Settings")]
    [SerializeField] private float generationThreshold = 10f;
    [SerializeField] private float cullingDistance = 20f;
    
    [Header("Movement Control")]
    [SerializeField] private float scrollSpeed = 5f; 

    [Header("Runtime Data")]
    private float worldCartesianX = 0f; 
    
    private int currentSortingOrder = 0;
    private Vector3 currentScrollOffset = Vector3.zero;

    private List<TerrainChunk> activeChunks = new List<TerrainChunk>();
    
    void Start()
    {
        if (playerTransform == null || terrainChunkPrefabs.Count == 0)
        {
            Debug.LogError("WorldManager setup incomplete. Check playerTransform and chunk prefabs.");
            return;
        }
        
        transform.position = Vector3.zero;

        for (int i = 0; i < 2; i++)
        {
            SpawnNewChunk();
        }
    }

    void LateUpdate()
    {
        UpdateWorldScroll();
        ApplyScrollOffset();
        CheckGeneration();
        CullOldChunks();
    }
    
    private void UpdateWorldScroll()
    {
        float scrollStep = scrollSpeed * Time.deltaTime;
        
        worldCartesianX += scrollStep;
        
        currentScrollOffset = Utils.CartesianToIsometric(new Vector3(-worldCartesianX, 0, 0));
    }
    private void ApplyScrollOffset()
    {
        transform.position = currentScrollOffset;
    }

    private void CheckGeneration()
    {
        if (activeChunks.Count == 0 || terrainChunkPrefabs.Count == 0) return;
        
        TerrainChunk lastChunk = activeChunks[activeChunks.Count - 1];
        float lastChunkEndAbsoluteX = lastChunk.CartesianStart.x + lastChunk.GetChunkLength();
        
        if (lastChunkEndAbsoluteX - worldCartesianX < generationThreshold)
        {
            SpawnNewChunk();
        }
    }
    
    private void SpawnNewChunk()
    {
        float spawnX = 0f;
        
        if (activeChunks.Count > 0)
        {
            TerrainChunk lastChunk = activeChunks[activeChunks.Count - 1];
            spawnX = lastChunk.CartesianStart.x + lastChunk.GetChunkLength();
        }
        

        TerrainChunk prefab = terrainChunkPrefabs[Random.Range(0, terrainChunkPrefabs.Count)];

        TerrainChunk newChunk = Instantiate(prefab, transform);

        newChunk.SetSortingOrder(currentSortingOrder);
        currentSortingOrder -= 1;
        
        Vector3 newCartesianStart = new Vector3(spawnX, 0, 0); 
        newChunk.CartesianStart = newCartesianStart; 

        Vector3 visualPos = Utils.CartesianToIsometric(newCartesianStart);
        
        newChunk.transform.localPosition = visualPos;
        
        activeChunks.Add(newChunk);
        Debug.Log($"Generated new chunk at Absolute Cartesian X: {spawnX}");
    }
    
    private void CullOldChunks()
    {
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            TerrainChunk chunk = activeChunks[i];
            
            float chunkEndAbsoluteX = chunk.CartesianStart.x + chunk.GetChunkLength();

            if (chunkEndAbsoluteX < worldCartesianX - cullingDistance)
            {
                activeChunks.RemoveAt(i);
                Destroy(chunk.gameObject);
                Debug.Log($"Culled old chunk (Index {i}) at Absolute Cartesian X: {chunk.CartesianStart.x}");
            }
        }
    }
    
    public float GetWorldCartesianX()
    {
        return worldCartesianX;
    }
}
