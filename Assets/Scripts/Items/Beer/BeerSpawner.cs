using UnityEngine;

public class BeerSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnBaseCartesianPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] private float spawnScatterRadius = 2f;
    [SerializeField] private int spawnCap = 10;
    [SerializeField] private float defaultInterval = 3f;
    [SerializeField] private float dizzyInterval = 2f;
    [SerializeField] private float drunkInterval = 1f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
