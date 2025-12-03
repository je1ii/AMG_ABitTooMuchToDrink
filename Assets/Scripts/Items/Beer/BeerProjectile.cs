using System.Collections.Generic;
using UnityEngine;

public class BeerProjectile : MonoBehaviour
{
    public Vector2 horizontalVelocity; 
    [SerializeField] private float gravity = 20f; 
    [SerializeField] private float groundY = 0f;  
    [SerializeField] private float lifeAfterStop = 0.5f;
    [SerializeField] private float hitRange = 0.5f;

    private EnemyHealth enemyTarget;
    private int bottleDamage = 1;
    
    private List<EnemyHealth> enemiesInScene;
    
    private bool landed = false;
    private float timer = 0f;
    
    private Vector3 worldPosition;
    
    private float verticalVelocity = 0f; 
    private float currentHeight = 0f;
    
    public void Initialize(Vector3 initialCartesianPosition, float initialVerticalVelocity, int damage)
    {
        worldPosition = new Vector3(initialCartesianPosition.x, initialCartesianPosition.y, 0f);
        
        currentHeight = initialCartesianPosition.z; 
        verticalVelocity = initialVerticalVelocity;
        enemiesInScene  = PlayerThrow.GetEnemies();
        bottleDamage = damage;
        
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
        
        UpdateEnemyTarget();
        CheckCollision();

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
    
    private void CheckCollision()
    {
        if(this.gameObject.name == "MolotovBottle")
        {
            for (int i = enemiesInScene.Count - 1; i >= 0; i--)
            {
                GameObject enemyGameObject = enemiesInScene[i].gameObject;
                float metric = Utils.CalculateMetric(this.gameObject,enemyGameObject.transform, hitRange);

                if (metric <= hitRange)
                {
                    enemiesInScene[i].TakeDamage(bottleDamage);
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            if (Utils.CalculateMetric(this.gameObject,enemyTarget.transform, hitRange) <= hitRange)
            {
                enemyTarget.TakeDamage(bottleDamage);
                Destroy(this.gameObject);
            }
        }
    }
    
    private void UpdateEnemyTarget()
    {
        enemyTarget = null;
        float closestMetric = float.MaxValue;

        foreach (EnemyHealth enemy in enemiesInScene)
        {
            GameObject enemyGameObject = enemy.gameObject;
            float metric = Utils.CalculateMetric(this.gameObject,enemyGameObject.transform, hitRange);
            
            if (metric < closestMetric)
            {
                closestMetric = metric;
                enemyTarget = enemy;
            }
        }
    }

    private void UpdateVisualPosition()
    {
        Vector3 isometricPosition = Utils.CartesianToIsometric(worldPosition);
        
        transform.position = new Vector3(
            isometricPosition.x, 
            isometricPosition.y + currentHeight, 
            0 
        );
    }
}
