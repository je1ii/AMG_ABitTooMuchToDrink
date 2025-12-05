using TMPro;
using UnityEngine;

public class InGameHUD : MonoBehaviour
{
    [Header("HUD Text References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI beerText;
    [SerializeField] private TextMeshProUGUI bottleText;
    [SerializeField] private TextMeshProUGUI molotovText;
    
    [Header("Player Inventory")]
    [SerializeField] private InventorySystem playerInventory;

    void Update()
    {
        UpdateHUD();
    }
    
    private void UpdateHUD()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{GameStats.GetScore()}";
        }

        if (timerText != null)
        {
            float currentTime = GameStats.GetCurrentTime();
            timerText.text = $"{currentTime:F2}s";
        }

        if (playerInventory != null)
        {
            InventorySlot beer = playerInventory.GetSlotOfType("Beer");
            InventorySlot bottle = playerInventory.GetSlotOfType("Empty Bottle");
            InventorySlot molotov = playerInventory.GetSlotOfType("Molotov");
            
            if (beer != null)
            {
                int beerAmount = beer.amount;
                beerText.text = $"{beerAmount}";
            }

            if (bottle != null)
            {
                int bottleAmount = bottle.amount;
                bottleText.text = $"{bottleAmount}";
            }
            
            if(molotov != null)
            {
                int molotovAmount = molotov.amount;
                molotovText.text = $"{molotovAmount}";
            }
        }
    }
}