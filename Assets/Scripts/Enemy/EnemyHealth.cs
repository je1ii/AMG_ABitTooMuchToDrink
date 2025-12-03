using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 1;

    private GameObject[] enemyDrops;
    
    public void AssignHealth(int hp) => health = hp;
    public void AssignDrops(GameObject[] drops) => enemyDrops = drops;
    
    private void OnEnable()
    {
        PlayerThrow.Register(this);
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        
        int dropsLength = enemyDrops.Length;
        
        if(dropsLength == 1)
            Instantiate(enemyDrops[0], transform.position, Quaternion.identity);
        
        int randomIndex = UnityEngine.Random.Range(0, dropsLength - 1);
        Instantiate(enemyDrops[randomIndex], transform.position, Quaternion.identity);
        
        PlayerThrow.Deregister(this);
        Destroy(gameObject);
    }
}
