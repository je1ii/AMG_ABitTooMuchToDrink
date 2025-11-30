using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumption : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private IntoxicationBar bar;
    
    [Header("Consumption Settings")]
    [SerializeField] private int normalCondition = 5;
    [SerializeField] private int fastCondition = 8;
    
    [SerializeField] private float slowTime = 15f;
    [SerializeField] private float normalTime = 8f;
    [SerializeField] private float fastTime = 3f;
    
    public bool isConsuming;
    private InventorySystem invSys;
    private PlayerHands hands;
    
    void Start()
    {
        invSys = GetComponent<InventorySystem>();
        hands = GetComponent<PlayerHands>();
    }
    
    // consumes one beer
    private IEnumerator Consuming()
    {
        while (true)
        {
            isConsuming = true;
            int totalBeer = invSys.TotalOfType("Beer");
            if (totalBeer <= 0)
            {
                isConsuming = false;
                yield break;
            }
            
            InventorySlot slot = invSys.FirstSlotOfType("Beer");
            BeerData beer = slot.TakeOne();
            if(hands.IsLeftEmpty) hands.HoldOnLeft(beer);
            
            Debug.Log("Consuming beer...");
            yield return new WaitForSeconds(SetCorrectTimer(totalBeer));
            
            bar.DoneConsuming(beer.intoxicationValue);
            if(!hands.IsLeftEmpty) hands.RemoveOnLeft();
            Debug.Log("Done consuming, added intoxication value: " + beer.intoxicationValue);
        }
    }

    private float SetCorrectTimer(int total)
    {
        if (total >= fastCondition) return fastTime;
        else if (total >= normalCondition) return normalTime;
        else return slowTime;
    }

    public void StartConsuming()
    {
        StartCoroutine(Consuming());
    }
}
