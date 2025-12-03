using UnityEngine;

    [CreateAssetMenu(fileName = "WaterData", menuName = "PowerUps/Water Bottle", order = 2)]
public class WaterData : PowerUpData
{
    [Header("Healing Properties")]
    [SerializeField] private int healAmount = 1;
    
    public override void ExecuteInteraction(GameObject interactor, GameObject item)
    {
        if (HealthBar.Instance.Heal(healAmount))
        {
            Debug.Log($"{interactor.name} got healed by {id}.");
            Destroy(item);
        }
        else
        {
            Debug.Log($"{interactor.name} health is full or is already dead.");
        }
    }
}
