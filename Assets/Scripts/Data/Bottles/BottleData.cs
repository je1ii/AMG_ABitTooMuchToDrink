using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BottleData", menuName = "Scriptable Objects/BottleData")]
public class BottleData: ScriptableObject
{
    public string id;
    public int amount;
    public Sprite bottleSprite;
    public Sprite iconUI;
    public GameObject bottleProjectile;
    public BottleData emptyBottle;
    public int intoxicationValue;
    public int damageValue;
}
    
