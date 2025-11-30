using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public BeerData beerData;
    public int amount;

    public InventorySlot(BeerData data, int amount)
    {
        beerData = data;
        this.amount = amount;
    }
}
