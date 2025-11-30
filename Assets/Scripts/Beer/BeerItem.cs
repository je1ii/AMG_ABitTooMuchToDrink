using UnityEngine;

public class BeerItem: MonoBehaviour
{
    [SerializeField] private BeerData beerData;
    [SerializeField] private SpriteRenderer sr;
    
    private static readonly int OutlineEnabled = Shader.PropertyToID("_OutlineEnabled");

    public BeerData FetchData()
    {
        return beerData;
    }
    
    // called in playerinteract script if this object is picked up
    public void PickUp()
    {
        Destroy(this.gameObject);
    }

    public void EnableOutline()
    {
        sr.material.SetFloat(OutlineEnabled, 1f);
    }

    public void DisableOutline()
    {
        sr.material.SetFloat(OutlineEnabled, 0f);
    }
}
