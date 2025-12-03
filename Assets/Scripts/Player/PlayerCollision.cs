using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float collideRange;
    private List<EnemyHealth> enemiesInScene;
    
    void Start()
    {
        enemiesInScene = PlayerThrow.GetEnemies();
    }

    void Update()
    {
        if(HealthBar.Instance.GetInvulnerableValue())
            CheckCollision();
    }

    private void CheckCollision()
    {
        for (int i = enemiesInScene.Count - 1; i >= 0; i--)
        {
            GameObject enemyGameObject = enemiesInScene[i].gameObject;
            float metric = Utils.CalculateMetric(this.gameObject, enemiesInScene[i].transform, collideRange);

            if (metric <= collideRange)
            {
                enemiesInScene[i].Die();
            }
        }
    }
}
