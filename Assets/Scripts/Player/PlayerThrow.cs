using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    private static readonly List<EnemyHealth> AllEnemies = new List<EnemyHealth>();
    [Header("Throw Settings")]
    [SerializeField] private float throwForwardSpeed = 8f;
    [SerializeField] private float throwUpwardSpeed = 0f;
    [SerializeField] private float throwDelay = 0.2f;
    [SerializeField] private float throwCooldown = 2f;
    
    private bool isThrowing = false;
    
    private InventorySystem invSys;
    private PlayerHands hands;
    private PlayerMovement movement;
    private PlayerConsumption consumption;
    
    public static int GetEnemiesCount() => AllEnemies.Count;
    public static List<EnemyHealth> GetEnemies() => AllEnemies;
    
    void Start()
    {
        invSys = GetComponent<InventorySystem>();
        hands = GetComponent<PlayerHands>();
        movement = GetComponent<PlayerMovement>();
        consumption = GetComponent<PlayerConsumption>();
    }

    public IEnumerator ThrowBeer()
    {
        BottleData toThrow = hands.Throwing();

        if (toThrow == null)
        {
            isThrowing = false;
            yield break;
        }

        yield return new WaitForSeconds(throwDelay);

        if (toThrow.bottleProjectile == null)
        {
            Debug.Log("Beer projectile is not assigned");
            isThrowing = false;
            yield break;
        }
            
        Vector2 flatDirection = movement.CartesianFacingDirection;
        Vector3 throwDirection = new Vector3(flatDirection.x, flatDirection.y, 0f); 

        Vector3 cartesianSpawnPos = movement.GetWorldPosition(); 
        
        cartesianSpawnPos = new Vector3(cartesianSpawnPos.x, cartesianSpawnPos.y, 1f);

        Vector3 visualSpawnPosBase = Utils.CartesianToIsometric(cartesianSpawnPos);
        Vector3 finalVisualSpawnPos = new Vector3(
            visualSpawnPosBase.x,
            visualSpawnPosBase.y + cartesianSpawnPos.z,
            0 
        );

        GameObject obj = Instantiate(
            toThrow.bottleProjectile,
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
        
        projectile.Initialize(cartesianSpawnPos, verticalThrowVelocity, toThrow.damageValue);
        projectile.horizontalVelocity = horizontalThrowVelocity;
        
        yield return new WaitForSeconds(throwCooldown);
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
        if(!consumption.isConsuming) return;
        
        int totalBeer = invSys.TotalAll();
        if (totalBeer <= 0) return;
        
        if (hands.IsRightEmpty)
        {
            InventorySlot slot = invSys.GetSlotOfType("Molotov");
            if (slot == null) slot = invSys.GetSlotOfType("Empty Bottle");
            if (slot == null) slot = invSys.GetSlotOfType("Beer");
            
            BottleData beer = slot.TakeOne();
            if (beer == null)return;
            
            hands.HoldOnRight(beer);
            Debug.Log(this.gameObject.name + $" equipped {beer.name} on their right hand.");
        }

        if (!hands.IsRightEmpty && (hands.rightHand.id == "Beer" || hands.rightHand.id == "Empty Bottle"))
        {
            InventorySlot slot = invSys.GetSlotOfType("Molotov");
            if (slot == null) slot = invSys.GetSlotOfType("Empty Bottle");
            if (slot == null) return;
            
            BottleData beer = slot.TakeOne();
            if (beer == null)return;

            BottleData beerOnHand = hands.TakeOnRight();
            invSys.AddItem(beerOnHand, beerOnHand.amount);
            
            hands.HoldOnRight(beer);
            Debug.Log(this.gameObject.name + $" switched {beer.name} on their right hand.");
        }
    }
    
    public static void Register(EnemyHealth enemy)
    {
        if (!AllEnemies.Contains(enemy))
        {
            AllEnemies.Add(enemy);
        }
    }

    public static void Deregister(EnemyHealth enemy)
    {
        AllEnemies.Remove(enemy);
    }
}