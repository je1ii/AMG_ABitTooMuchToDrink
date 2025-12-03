using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private static readonly List<ObstacleItem> AllObstacles = new List<ObstacleItem>();
    
    [SerializeField] private float collideRange;
    [SerializeField] private float obstacleDamageInterval = 3f;
    
    private float nextObstacleDamageTime = 0f;
    private float obstacleRange;
    
    private List<EnemyHealth> enemiesInScene;
    private ObstacleItem nearObstacle;

    private PlayerMovement pm;
    
    void Start()
    {
        enemiesInScene = PlayerThrow.GetEnemies();
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(HealthBar.Instance.GetInvulnerableValue())
            CheckEnemyCollision();
        
        UpdateNearObstacle();
        CheckObstacleCollision();
    }

    private void CheckEnemyCollision()
    {
        for (int i = enemiesInScene.Count - 1; i >= 0; i--)
        {
            EnemyHealth enemyGameObject = enemiesInScene[i];
            if (enemyGameObject == null) continue; 
            float metric = Utils.CalculateMetric(this.gameObject, enemyGameObject.transform, collideRange);

            if (metric <= collideRange)
            {
                enemyGameObject.Die();
            }
        }
    }

    private void CheckObstacleCollision()
    {
        if (nearObstacle == null) return;
        
        obstacleRange = nearObstacle.GetData().range;
        if (Utils.CalculateMetric(this.gameObject, nearObstacle.transform, obstacleRange) <= obstacleRange)
        {
            ObstacleData data = nearObstacle.GetData();
            if(!pm.GetIsJumping())
            {
                if (data.deadly)
                {
                    HealthBar.Instance.KillPlayer();
                }

                if (Time.time >= nextObstacleDamageTime)
                {
                    HealthBar.Instance.TakeDamage(data.damage);
                    Debug.Log($"{this.name} hit an obstacle: {data.name}. Took {data.damage} damage.");

                    nextObstacleDamageTime = Time.time + obstacleDamageInterval;
                }
            }
            else if (data.deadly && data.notJumpable)
            {
                HealthBar.Instance.KillPlayer();
            }
        }
    }
    
    private void UpdateNearObstacle()
    {
        nearObstacle = null;
        float closestMetric = float.MaxValue;

        foreach (ObstacleItem obs in AllObstacles)
        {
            if (obs == null) continue;
            GameObject obsGameObject = obs.gameObject;
            float metric = Utils.CalculateMetric(this.gameObject, obsGameObject.transform, collideRange);
            
            if (metric < closestMetric)
            {
                closestMetric = metric;
                nearObstacle = obs;
            }
        }
    }
    
    public static void Register(ObstacleItem obstacle)
    {
        if (!AllObstacles.Contains(obstacle))
        {
            AllObstacles.Add(obstacle);
        }
    }

    public static void Deregister(ObstacleItem obstacle)
    {
        AllObstacles.Remove(obstacle);
    }
}
