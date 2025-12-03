using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BeerItem: MonoBehaviour, IInteractable
{
    [SerializeField] private BottleData bottleData;
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
        InventorySystem inv = interactor.GetComponent<InventorySystem>();
        if(inv.AddItem(bottleData, bottleData.amount))
        {
            Debug.Log(interactor.name + $" added {bottleData.name} to their inventory.");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log(interactor.name + "'s inventory is full.");
            // indicate full
        }
    }
}
