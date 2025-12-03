using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class InventorySlot
{
    public BottleData bottleData;
    public int amount;
    
    public bool hasBeer => bottleData != null && amount > 0;

    public InventorySlot(BottleData beer, int amount)
    {
        bottleData = beer;
        this.amount = amount;
    }

    public BottleData TakeOne()
    {
        if (!hasBeer) return null;
        
        amount--;
        return bottleData;
    }
}
