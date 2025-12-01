using UnityEngine;

public class BeerProjectile : MonoBehaviour
{
    public Vector2 horizontalVelocity; 
    [SerializeField] private float gravity = 20f; 
    [SerializeField] private float groundY = 0f;  
    [SerializeField] private float lifeAfterStop = 0.5f;

    private bool landed = false;
    private float timer = 0f;
    
    private Vector3 worldPosition;
    
    private float verticalVelocity = 0f; 
    private float currentHeight = 0f;
    
    public void Initialize(Vector3 initialCartesianPosition, float initialVerticalVelocity)
    {
        worldPosition = new Vector3(initialCartesianPosition.x, initialCartesianPosition.y, 0f);
        
        currentHeight = initialCartesianPosition.z; 
        verticalVelocity = initialVerticalVelocity;
        
        UpdateVisualPosition();
    }

    void Update()
    {
        if (landed)
        {
            timer += Time.deltaTime;
            if (timer >= lifeAfterStop)
                Destroy(gameObject);
            return;
        }

        worldPosition.x += horizontalVelocity.x * Time.deltaTime;
        worldPosition.y += horizontalVelocity.y * Time.deltaTime;
        
        verticalVelocity -= gravity * Time.deltaTime; 
        currentHeight += verticalVelocity * Time.deltaTime;
        
        if (currentHeight <= groundY)
        {
            currentHeight = groundY;
            horizontalVelocity = Vector2.zero;
            verticalVelocity = 0f;
            landed = true;
        }
        
        UpdateVisualPosition();
    }

    private void UpdateVisualPosition()
    {
        Vector3 isometricPosition = IsometricUtil.CartesianToIsometric(worldPosition);
        
        transform.position = new Vector3(
            isometricPosition.x, 
            isometricPosition.y + currentHeight, 
            0 
        );
    }
}
