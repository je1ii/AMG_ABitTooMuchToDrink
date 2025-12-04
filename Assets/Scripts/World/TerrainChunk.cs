using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainChunk : MonoBehaviour
{
    [Tooltip("The length of this chunk along the forward (Y) axis in Cartesian units.")]
    [SerializeField] private float chunkLength = 10f;
    [SerializeField] private TilemapRenderer tr;

    // The starting Cartesian position of this chunk (set by WorldManager)
    public Vector3 CartesianStart { get; set; }

    public void SetSortingOrder(int order)
    {
        tr.sortingOrder = order;
    }

    public float GetChunkLength()
    {
        return chunkLength;
    }
}
