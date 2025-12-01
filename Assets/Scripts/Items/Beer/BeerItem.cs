using System.Collections.Generic;
using UnityEngine;

public class BeerItem: MonoBehaviour, IInteractable
{
    [SerializeField] private BeerData beerData;
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
        if(inv.AddItem(beerData, beerData.beerAmount))
        {
            Debug.Log(interactor.name + $" added {beerData.name} to their inventory.");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log(interactor.name + "'s inventory is full.");
            // indicate full
        }
    }
}
