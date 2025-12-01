using UnityEngine;

public abstract class PowerUpData : ScriptableObject
{
    [Header("Base Interaction Data")]
    public string ItemName = "Power-Up";
    public string InteractionVerb = "Activate";

    public abstract void ExecuteInteraction(GameObject interactor);
}
