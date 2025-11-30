using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private List<GameObject> beersInRange = new List<GameObject>();

    [SerializeField] private GameObject interactTarget;
    private InventorySystem invSys;

    private void Start()
    {
        invSys = GetComponentInChildren<InventorySystem>();
    }

    void Update()
    {
        UpdateBeersInRange();     
        UpdateInteractTarget();   
        
        // collect beer in range
        if (Input.GetKeyDown(KeyCode.E))
            CollectBeer();
    }
    
    // update the list of beers in range of the player
    private void UpdateBeersInRange()
    {
        GameObject[] beersInScene = GameObject.FindGameObjectsWithTag("Beer");
        
        beersInRange.Clear();
        
        foreach (GameObject beer in beersInScene)
        {
            if (beer == null) continue;

            float distance = FindDistance(beer.transform);

            BeerItem beerItem = beer.GetComponent<BeerItem>();

            if (distance <= interactRange)
            {
                beersInRange.Add(beer);
                beerItem?.EnableOutline();
            }
            else
            {
                beerItem?.DisableOutline();
            }
        }
    }
    
    // update player's target for interaction
    private void UpdateInteractTarget()
    {
        interactTarget = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject beer in beersInRange)
        {
            float distance = FindDistance(beer.transform);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                interactTarget = beer;
            }
        }
    }
    
    // find distance of a target and the transform where the script is attached
    private float FindDistance(Transform target)
    {
        Vector3 a = transform.position;
        Vector3 b = target.position;

        float dx = b.x - a.x;
        float dy = b.y - a.y; 

        float rx = interactRange;
        
        float ry = interactRange * 0.5f;

        float metric = (dx * dx) / (rx * rx) + (dy * dy) / (ry * ry);
        return metric;
    }

    private void CollectBeer()
    {
        if (interactTarget == null) return;
        
        BeerItem beerItem = interactTarget.GetComponent<BeerItem>();
        if (beerItem == null) return;
        
        BeerData data = beerItem.FetchData();
        if(invSys.AddItem(data, data.beerAmount)) Debug.Log("Added to inventory: " + interactTarget.name);

        beersInRange.Remove(interactTarget);
        beerItem.PickUp();
        
        interactTarget = null;
    }
    
    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, interactRange);
        
        Gizmos.color = Color.red;
        Vector3 center = transform.position;
        int segments = 32;
        
        const float isometricSquashFactor = 0.5f; 

        Vector3 lastPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * (360f / segments) * Mathf.Deg2Rad;
            
            float x = interactRange * Mathf.Cos(angle);
            float y = (interactRange * isometricSquashFactor) * Mathf.Sin(angle);
            
            Vector3 currentPoint = new Vector3(center.x + x, center.y + y, center.z); 

            if (i == 0)
            {
                firstPoint = currentPoint;
            }
            
            if (i > 0)
            {
                Gizmos.DrawLine(lastPoint, currentPoint);
            }
            
            lastPoint = currentPoint;
        }

        Gizmos.DrawLine(lastPoint, firstPoint);
    }
}
