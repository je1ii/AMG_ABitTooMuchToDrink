using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
    private BottleData takenBeer;
    private bool isBeerTaken = false;
    private InventorySystem invSys;
    private PlayerHands hands;
    
    void Start()
    {
        invSys = GetComponent<InventorySystem>();
        hands = GetComponent<PlayerHands>();
    }

    void Update()
    {
        int totalBeer = invSys.TotalOfType("Beer");
        if (!isConsuming && totalBeer > 0 && !HealthBar.Instance.GetInvulnerableValue())
        {
            StartCoroutine(Consuming());
        }
        
        // if consuming but became invulnerable, put back beer in inventory
        if (HealthBar.Instance.GetInvulnerableValue() && isConsuming)
        {
            if(isBeerTaken)
            {
                invSys.AddItem(takenBeer, takenBeer.amount);
                isBeerTaken = false;
                takenBeer = null;
            }

            isConsuming = false;
            StopCoroutine(Consuming());
        }
        
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
            
            InventorySlot slot = invSys.GetSlotOfType("Beer");
            takenBeer = slot.TakeOne();
            
            if(takenBeer != null)
                isBeerTaken = true;
            
            if(hands.IsLeftEmpty) hands.HoldOnLeft(takenBeer);
            
            Debug.Log("Consuming beer...");
            yield return new WaitForSeconds(SetCorrectTimer(totalBeer));
            
            if(slot != null) 
                invSys.AddItem(takenBeer.emptyBottle, takenBeer.emptyBottle.amount);
            
            bar.DoneConsuming(takenBeer.intoxicationValue);
            if(!hands.IsLeftEmpty) hands.RemoveOnLeft();
            Debug.Log("Done consuming, added intoxication value: " + takenBeer.intoxicationValue);
            
            takenBeer = null;
            isBeerTaken = false;
        }
    }

    private float SetCorrectTimer(int total)
    {
        if (total >= fastCondition) return fastTime;
        else if (total >= normalCondition) return normalTime;
        else return slowTime;
    }
}
