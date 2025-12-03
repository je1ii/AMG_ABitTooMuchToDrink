using UnityEngine;

public class PowerUpItem : MonoBehaviour, IInteractable
{
    [SerializeField] private PowerUpData powerUpData;
    [SerializeField] private SpriteRenderer sr;
    
    private void OnEnable()
    {
        PlayerInteract.Register(this);
    }
    
    private void OnDisable()
    {
        PlayerInteract.Deregister(this);
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void EnableOutline()
    {
        sr.material.SetFloat("_OutlineEnabled", 1f);
    }

    public void DisableOutline()
    {
        sr.material.SetFloat("_OutlineEnabled", 0f);
    }

    public void Interact(GameObject interactor)
    {
        if (powerUpData != null)
        {
            powerUpData.ExecuteInteraction(interactor, this.gameObject);
        }
        else
        {
            Debug.LogError($"{gameObject.name} is missing a PowerUpData reference!");
        }
    }
}
