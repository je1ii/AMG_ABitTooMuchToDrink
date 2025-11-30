using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public BeerData beerData;
    public int amount;
    
    public bool hasBeer => beerData != null && amount > 0;

    public InventorySlot(BeerData data, int amount)
    {
        beerData = data;
        this.amount = amount;
    }

    public BeerData TakeOne()
    {
        if (!hasBeer) return null;
        
        amount--;
        return beerData;
    }
}
