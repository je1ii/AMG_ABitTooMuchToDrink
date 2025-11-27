using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 0;
    void Start()
    {
        
    }
    
    void Update()
    {
        if(this != null) InputMovement();
    }
    
    private void InputMovement()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        
        Vector3 movement = new Vector3(0, 0, 0)
        {
            x = vInput,
            y = -hInput
        };
        
        if(movement.sqrMagnitude > 1) movement.Normalize();
        
        Vector3 isometricMovement = CartesianToIsometric(movement).normalized;
        Vector3 motion = isometricMovement * (moveSpeed * Time.deltaTime);

        transform.position += motion;
    }

    private Vector3 CartesianToIsometric(Vector3 cartesian)
    {
        Vector3 screenPos = new Vector3(0, 0, 0)
        {
            x = cartesian.x - cartesian.y,
            y = (cartesian.x + cartesian.y) / 2
        };
        return screenPos;
    }
}
