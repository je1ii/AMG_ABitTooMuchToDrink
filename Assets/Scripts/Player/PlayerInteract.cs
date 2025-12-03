using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private static readonly List<IInteractable> allInteractables = new List<IInteractable>();
    
    private const float IsometricSquashFactor = 0.5f; 
    
    [SerializeField] private float interactRange = 2f;
    private List<IInteractable> itemsInRange = new List<IInteractable>();

    private IInteractable interactTarget;
    private PlayerThrow pt;

    private void Start()
    {
        pt = GetComponent<PlayerThrow>();
    }

    void Update()
    {
        ProcessInteractions();     
        UpdateInteractTarget();
        pt.ReadyThrow();
        
        // collect beer in range
        if (Input.GetKeyDown(KeyCode.E))
            PickedUpTarget();
        
        // throw beer from inventory
        if (Input.GetMouseButtonDown(0))
            pt.StartThrow();
    }
        
    
    // update the list of beers in range of the player
    private void ProcessInteractions()
    {
        itemsInRange.Clear();
        
        foreach (IInteractable item in allInteractables)
        {
            if (item == null) continue;
            
            GameObject itemObject = item.GetGameObject();
            float metric = CalculateMetric(itemObject.transform);
        
            if (metric <= 1f)
            {
                itemsInRange.Add(item);
                item.EnableOutline();
            }
            else
            {
                item.DisableOutline();
            }
        }
    }
    
    // update player's target for interaction
    private void UpdateInteractTarget()
    {
        interactTarget = null;
        float closestMetric = float.MaxValue;

        foreach (IInteractable item in itemsInRange)
        {
            GameObject itemObject = item.GetGameObject();
            float metric = CalculateMetric(itemObject.transform);
            
            if (metric < closestMetric)
            {
                closestMetric = metric;
                interactTarget = item;
            }
        }
    }

    private void PickedUpTarget()
    {
        if (interactTarget == null) return;
        
        interactTarget.Interact(this.gameObject);
        
        interactTarget = null;
    }
    
    // calculate metric from the center of the elliptical range
    private float CalculateMetric(Transform target)
    {
        Vector3 a = transform.position;
        Vector3 b = target.position; 

        float dx = b.x - a.x;
        float dy = b.y - a.y; 

        float rx = interactRange;
        float ry = interactRange * IsometricSquashFactor;

        float metric = (dx * dx) / (rx * rx) + (dy * dy) / (ry * ry);
        return metric;
    }
    
    public static void Register(IInteractable interactable)
    {
        if (!allInteractables.Contains(interactable))
        {
            allInteractables.Add(interactable);
        }
    }

    public static void Deregister(IInteractable interactable)
    {
        allInteractables.Remove(interactable);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position;
        int segments = 32;
        
        const float isometricSquashFactor = IsometricSquashFactor; // Use the constant

        Vector3 lastPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * (360f / segments) * Mathf.Deg2Rad;
            
            float x = interactRange * Mathf.Cos(angle);
            float y = (interactRange * isometricSquashFactor) * Mathf.Sin(angle);
            
            // Draw at the visual position (transform.position is already isometric screen space)
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

        // Close the loop
        Gizmos.DrawLine(lastPoint, firstPoint);
    }
}
