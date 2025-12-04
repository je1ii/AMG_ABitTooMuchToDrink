using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 1;

    private GameObject[] enemyDrops;
    private Transform parent;
    
    public void AssignHealth(int hp) => health = hp;
    public void AssignDrops(GameObject[] drops) => enemyDrops = drops;
    public void AssignParent(Transform p) => parent = p;
    
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
        
        int randomIndex = UnityEngine.Random.Range(0, dropsLength);
        Instantiate(enemyDrops[randomIndex], transform.position, Quaternion.identity, parent);
        
        PlayerThrow.Deregister(this);
        Destroy(gameObject);
    }
}
