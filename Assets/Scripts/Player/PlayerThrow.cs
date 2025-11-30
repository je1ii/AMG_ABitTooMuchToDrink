using System.Collections;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private float throwForwardSpeed = 8f;
    [SerializeField] private float throwUpwardSpeed = 3f;
    [SerializeField] private float throwDelay = 1f;
    
    private bool isThrowing = false;
    
    private PlayerHands hands;
    private InventorySystem invSys;
    void Start()
    {
        invSys = GetComponent<InventorySystem>();
        hands = GetComponent<PlayerHands>();
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
            
        GameObject obj = Instantiate(
            toThrow.beerProjectile,
            transform.position + transform.forward * 0.5f + Vector3.up * 1f,
            Quaternion.identity
        );

        BeerProjectile projectile = obj.GetComponent<BeerProjectile>();

        if (projectile == null)
        {
            Debug.Log("Beer projectile script is not attached");
            isThrowing = false;
            yield break;
        }

        // Vector2 isoDir = movementIso.LastMoveDirection;
        //
        // // if not moving, fallback to top-down orientation
        // if (isoDir.sqrMagnitude < 0.01f)
        //     isoDir = movementIso.FacingDirection;

        // Vector3 forward = new Vector3(isoDir.x, 0f, isoDir.y).normalized;

        // projectile.velocity =
        //     forward * throwForwardSpeed +
        //     Vector3.up * throwUpwardSpeed;

        isThrowing = false;
    }

    public void StartThrow()
    {
        if (!isThrowing && !hands.IsRightEmpty)
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