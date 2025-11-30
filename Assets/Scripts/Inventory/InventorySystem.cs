using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private int maxSlots = 3;

    private PlayerConsumption pc;
    
    // add item in player's inventory
    public bool AddItem(BeerData beerData, int amount)
    {
        pc = GetComponent<PlayerConsumption>();
        foreach (var slot in inventorySlots)
        {
            if (slot.beerData == beerData)
            {
                slot.amount += amount;
                if(!pc.isConsuming) pc.StartConsuming();
                return true;
            }
        }

        if (inventorySlots.Count < maxSlots)
        {
            inventorySlots.Add(new InventorySlot(beerData, amount));
            if(!pc.isConsuming) pc.StartConsuming();
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
            if (slot.beerData != null)
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
            if (slot.beerData != null && slot.beerData.id == beerID)
            {
                total += slot.amount;
            }
        }

        return total;
    }
    
    public InventorySlot FirstSlotOfType(string beerID)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.beerData != null && slot.beerData.id == beerID && slot.amount > 0)
            {
                return slot;
            }
        }
        return null;
    }
}
