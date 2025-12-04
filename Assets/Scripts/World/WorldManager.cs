using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The Player's Transform. MUST NOT be a child of this WorldRoot.")]
    [SerializeField] private Transform playerTransform;
    
    [Tooltip("List of all possible street chunk prefabs to choose from.")]
    [SerializeField] private List<TerrainChunk> terrainChunkPrefabs;

    [Header("Generation Settings")]
    [Tooltip("The safety buffer (Cartesian X) between the player and the last chunk.")]
    [SerializeField] private float generationThreshold = 10f;
    [Tooltip("Distance behind the player (Cartesian X) where chunks are destroyed.")]
    [SerializeField] private float cullingDistance = 20f;
    
    [Header("Movement Control (Continuous Scroll)")]
    [Tooltip("The constant speed (Cartesian units/sec) the world scrolls forward.")]
    [SerializeField] private float scrollSpeed = 5f; 

    [Header("Runtime Data")]
    // Tracks the total Cartesian X distance the world has scrolled (our new forward axis).
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
        
        // CRITICAL SETUP: Ensure the WorldRoot transform remains at global origin
        transform.position = Vector3.zero;

        // Initialize by spawning a few chunks
        for (int i = 0; i < 2; i++)
        {
            SpawnNewChunk();
        }
    }

    void LateUpdate()
    {
        // 1. Update Scroll Offset (Scrolls constantly forward)
        UpdateWorldScroll();
        
        // 2. Apply Scroll Offset (THIS MOVES THE WORLD)
        ApplyScrollOffset();

        // 3. Check Generation Threshold
        CheckGeneration();

        // 4. Check Culling Threshold
        CullOldChunks();
    }
    
    /// <summary>
    /// Calculates the constant forward scroll amount and updates the conceptual world position (X-axis).
    /// </summary>
    private void UpdateWorldScroll()
    {
        // Calculate the constant scroll step for this frame.
        float scrollStep = scrollSpeed * Time.deltaTime;
        
        // 1. Update the total conceptual distance the world has scrolled forward (along X).
        worldCartesianX += scrollStep;
        
        // 2. Convert the new worldCartesianX into the final Isometric scroll offset
        // Assuming forward movement (positive X) means the world must visually shift to the left/down (negative X in isometric view).
        // The Y and Z components remain 0 in Cartesian space for the scroll vector.
        currentScrollOffset = Utils.CartesianToIsometric(new Vector3(-worldCartesianX, 0, 0));
    }
    
    /// <summary>
    /// Applies the calculated isometric scroll offset to the WorldRoot.
    /// This moves the entire environment (chunks, enemies, etc.).
    /// </summary>
    private void ApplyScrollOffset()
    {
        // Setting the transform position instantly moves all children
        transform.position = currentScrollOffset;
    }

    /// <summary>
    /// Checks if a new chunk needs to be spawned based on the X-axis position.
    /// </summary>
    private void CheckGeneration()
    {
        if (activeChunks.Count == 0 || terrainChunkPrefabs.Count == 0) return;
        
        TerrainChunk lastChunk = activeChunks[activeChunks.Count - 1];
        // Check the end of the last chunk's absolute X position
        float lastChunkEndAbsoluteX = lastChunk.CartesianStart.x + lastChunk.GetChunkLength();
        
        // We check the absolute end of the last chunk against the total distance the world has scrolled (worldCartesianX).
        if (lastChunkEndAbsoluteX - worldCartesianX < generationThreshold)
        {
            SpawnNewChunk();
        }
    }

    /// <summary>
    /// Spawns a new chunk and places it at the end of the last chunk along the X-axis.
    /// </summary>
    private void SpawnNewChunk()
    {
        // Determine the spawn position (Absolute Cartesian X)
        float spawnX = 0f;
        
        if (activeChunks.Count > 0)
        {
            TerrainChunk lastChunk = activeChunks[activeChunks.Count - 1];
            // The spawn X is the end of the last chunk
            spawnX = lastChunk.CartesianStart.x + lastChunk.GetChunkLength();
        }
        // If activeChunks.Count == 0, spawnX remains 0, starting the chain correctly.

        TerrainChunk prefab = terrainChunkPrefabs[Random.Range(0, terrainChunkPrefabs.Count)];

        // Instantiate the chunk as a child of WorldRoot (this transform)
        TerrainChunk newChunk = Instantiate(prefab, transform);

        newChunk.SetSortingOrder(currentSortingOrder);
        currentSortingOrder -= 1;
        
        // Set its absolute Cartesian start point (along the X-axis)
        Vector3 newCartesianStart = new Vector3(spawnX, 0, 0); // X is the forward axis, Y is the fixed lateral axis
        newChunk.CartesianStart = newCartesianStart; 

        // Convert the Cartesian start position to the visual Isometric position
        Vector3 visualPos = Utils.CartesianToIsometric(newCartesianStart);
        
        // The chunk's local position is based on the calculated visual position relative to the WorldRoot.
        newChunk.transform.localPosition = visualPos;
        
        activeChunks.Add(newChunk);
        Debug.Log($"Generated new chunk at Absolute Cartesian X: {spawnX}");
    }

    /// <summary>
    /// Destroys chunks that have passed the camera/player by the culling distance (X-axis).
    /// </summary>
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
