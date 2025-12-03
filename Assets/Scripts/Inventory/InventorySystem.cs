using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private int maxSlots = 3;
    
    // add item in player's inventory
    public bool AddItem(BottleData beer, int amount)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.bottleData.id == beer.id)
            {
                slot.amount += amount;
                return true;
            }
        }

        if (inventorySlots.Count < maxSlots)
        {
            inventorySlots.Add(new InventorySlot(beer, amount));
            return true;
        }
        
        Debug.Log("Inventory full.");
        return false;
    }

    public int TotalAll()
    {
        int total = 0;

        foreach (var slot in inventorySlots)
        {
            if (slot.bottleData != null)
            {
                total += slot.amount;
            }
        }

        return total;
    }
    
    public int TotalOfType(string beerID)
    {
        int total = 0;
        
        foreach (var slot in inventorySlots)
        {
            if (slot.bottleData != null && slot.bottleData.id == beerID)
            {
                total += slot.amount;
            }
        }

        return total;
    }
    
    public InventorySlot GetSlotOfType(string beerID)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.bottleData != null && slot.bottleData.id == beerID && slot.amount > 0)
            {
                return slot;
            }
        }
        
        return null;
    }
}
