using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BeerData", menuName = "Scriptable Objects/BeerData")]
public class BeerData: ScriptableObject
{
    public string id;
    public int beerAmount;
    public Sprite beerSprite;
    public Sprite iconUI;
    public GameObject beerProjectile;
    public int intoxicationValue;
    public int damageValue;
}
    
