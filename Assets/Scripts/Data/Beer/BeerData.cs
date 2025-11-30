using UnityEngine;

[CreateAssetMenu(fileName = "BeerData", menuName = "Scriptable Objects/BeerData")]
public class BeerData: ScriptableObject
{
    public string beerName;
    public int beerAmount;
    public Sprite beerSprite;
    public Sprite iconUI;
    public int intoxicationValue;
}
    
