using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private int maxSlots = 3;
    
    // add item in player's inventory
    public bool AddItem(BeerData beerData, int amount)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.beerData == beerData)
            {
                slot.amount += amount;
                return true;
            }
        }

        if (inventorySlots.Count < maxSlots)
        {
            inventorySlots.Add(new InventorySlot(beerData, amount));
            return true;
        }
        
        Debug.Log("Inventory full.");
        return false;
    }
}
