using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainChunk : MonoBehaviour
{
    [SerializeField] private float chunkLength = 10f;
    [SerializeField] private TilemapRenderer tr;

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
