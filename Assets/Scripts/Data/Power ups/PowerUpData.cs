using UnityEngine;

public abstract class PowerUpData : ScriptableObject
{
    [Header("Base Interaction Data")]
    public string id = "PowerUp";
    

    public abstract void ExecuteInteraction(GameObject interactor, GameObject item);
}
