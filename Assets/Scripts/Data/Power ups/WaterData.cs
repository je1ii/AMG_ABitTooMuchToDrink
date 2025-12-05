using UnityEngine;

    [CreateAssetMenu(fileName = "WaterData", menuName = "PowerUps/Water Bottle", order = 2)]
public class WaterData : PowerUpData
{
    [Header("Healing Properties")]
    [SerializeField] private int healAmount = 1;
    [SerializeField] private float soberAmount = 10f;
    
    public override void ExecuteInteraction(GameObject interactor, GameObject item)
    {
        bool canSober = IntoxicationBar.Instance.SoberUp(soberAmount);
        bool canHeal = HealthBar.Instance.Heal(healAmount);
        
        if (canHeal)
        {
            
            Debug.Log($"{interactor.name} got healed by {id}.");
            Destroy(item);
        }
        else
        {
            Debug.Log($"{interactor.name} health is full or is already dead.");
        }
        
        if (canSober)
        {
            Debug.Log($"{interactor.name} sobered up with {id}.");
            Destroy(item);
        }
        else
        {
            Debug.Log($"{interactor.name} is sober.");
        }
    }
}
