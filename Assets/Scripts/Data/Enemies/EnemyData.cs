using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Details")] 
    public string enemyID;
    public GameObject prefab;
    public Sprite[] sprites;

    [Header("Base Stats")] 
    public int health = 1;
    public int damage = 1;
    public float speed = 3f;
    public float spawnWeight = 2f;
    public GameObject[] drops;
}
