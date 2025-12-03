using UnityEngine;

[CreateAssetMenu(fileName = "CoffeeData", menuName = "PowerUps/Coffee", order = 3)]
public class CoffeeData: PowerUpData
{
    [Header("Coffee Settings")]
    [SerializeField] private float invulnerableDuration = 3f;
    public override void ExecuteInteraction(GameObject interactor, GameObject item)
    {
        HealthBar.Instance.SetInvulnerable(true);
        HealthBar.Instance.StartTimer(invulnerableDuration);
        Destroy(item);        
    }
}
