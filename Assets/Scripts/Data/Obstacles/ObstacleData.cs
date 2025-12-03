using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Scriptable Objects/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    public string id;
    public float range;
    public float damage;
    public bool deadly;
    public bool notJumpable;
}
