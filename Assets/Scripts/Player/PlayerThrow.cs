using System.Collections;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private float throwForwardSpeed = 8f;
    [SerializeField] private float throwUpwardSpeed = 0f;
    [SerializeField] private float throwDelay = 0.2f;
    
    private bool isThrowing = false;
    
    private PlayerHands hands;
    private InventorySystem invSys;
    private PlayerMovement movement;
    void Start()
    {
        invSys = GetComponent<InventorySystem>();
        hands = GetComponent<PlayerHands>();
        movement = GetComponent<PlayerMovement>();
    }
    
    public IEnumerator ThrowBeer()
    {
        BeerData toThrow = hands.Throwing();

        if (toThrow == null)
        {
            isThrowing = false;
            yield break;
        }

        yield return new WaitForSeconds(throwDelay);

        if (toThrow.beerProjectile == null)
        {
            Debug.Log("Beer projectile is not assigned");
            isThrowing = false;
            yield break;
        }
            
        Vector2 flatDirection = movement.CartesianFacingDirection;
        Vector3 throwDirection = new Vector3(flatDirection.x, flatDirection.y, 0f); 

        Vector3 cartesianSpawnPos = movement.GetWorldPosition(); 
        
        cartesianSpawnPos = new Vector3(cartesianSpawnPos.x, cartesianSpawnPos.y, 1f);

        Vector3 visualSpawnPosBase = IsometricUtil.CartesianToIsometric(cartesianSpawnPos);
        Vector3 finalVisualSpawnPos = new Vector3(
            visualSpawnPosBase.x,
            visualSpawnPosBase.y + cartesianSpawnPos.z,
            0 
        );

        GameObject obj = Instantiate(
            toThrow.beerProjectile,
            finalVisualSpawnPos,
            Quaternion.identity
        );

        BeerProjectile projectile = obj.GetComponent<BeerProjectile>();

        if (projectile == null)
        {
            Debug.LogError("Beer projectile script is not attached to the prefab.");
            isThrowing = false;
            Destroy(obj);
            yield break;
        }
        
        Vector2 horizontalThrowVelocity = new Vector2(
            throwDirection.x * throwForwardSpeed, 
            throwDirection.y * throwForwardSpeed
        );
        
        float verticalThrowVelocity = throwUpwardSpeed;
        
        projectile.Initialize(cartesianSpawnPos, verticalThrowVelocity);
        projectile.horizontalVelocity = horizontalThrowVelocity;
        
        isThrowing = false;
    }

    public void StartThrow()
    {
        if (!isThrowing && hands != null && !hands.IsRightEmpty)
        {
            isThrowing = true;
            StartCoroutine(ThrowBeer());
        }
    }

    public void ReadyThrow()
    {
        int totalBeer = invSys.TotalAll();
        if (totalBeer <= 0) return;
        
        if (hands.IsRightEmpty)
        {
            InventorySlot slot = invSys.FirstSlotOfType("Molotov");
            if (slot == null) slot = invSys.FirstSlotOfType("Beer");
            
            BeerData beer = slot.TakeOne();
            if (beer == null)return;
            
            hands.HoldOnRight(beer);
        }
    }
}