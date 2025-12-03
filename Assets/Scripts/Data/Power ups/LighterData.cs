using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "LighterData", menuName = "PowerUps/Lighter", order = 1)]
public class LighterData : PowerUpData
{
    [Header("Upgrade Bottle Reference")]
    [SerializeField] private BottleData molotovData;
    
    public override void ExecuteInteraction(GameObject interactor, GameObject item)
    {
        InventorySystem inv = interactor.GetComponent<InventorySystem>();
        int totalBeer = inv.TotalOfType("Beer");
        if (totalBeer > 0)
        {
            InventorySlot slot = inv.GetSlotOfType("Beer");
            slot.TakeOne();
            if(molotovData == null)
            {
                Debug.Log("Upgrade bottle reference is not assigned.");
                return;
            }
            
            if (inv.AddItem(molotovData, molotovData.amount))
            {
                Debug.Log($"{interactor.name} added {molotovData.id} to their inventory.");
            }
            Destroy(item);
        }
        else
        {
            Debug.Log($"{interactor.name} tried to pick up a lighter but theres no bottle in their inventory!");
        }
        
    }
}
