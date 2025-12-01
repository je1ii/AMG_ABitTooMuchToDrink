using UnityEngine;

public class WaterData : PowerUpData
{
    [Header("Healing Properties")]
    [SerializeField] private int healAmount = 1;
    
    public override void ExecuteInteraction(GameObject interactor)
    {
        throw new System.NotImplementedException();
    }
}
