using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 20f;
    
    [SerializeField] private Transform spriteTransform;
    
    private bool isJumping = false;
    private float verticalVelocity = 0f;
    private float currentHeight = 0f;  
    
    void Update()
    {
        if (this != null)
        {
            InputMovement();
            HandleJump();
            
            if (spriteTransform == null)
            {
                Debug.Log("Sprite not assigned.");
                return;
            }
            
            ApplyVisualHeight();
        }
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
    
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentHeight == 0)
        {
            verticalVelocity = jumpForce;
            isJumping = true;
        }

        if (currentHeight > 0 || verticalVelocity > 0)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            currentHeight += verticalVelocity * Time.deltaTime;

            if (currentHeight < 0)
            {
                currentHeight = 0;
                verticalVelocity = 0;
                isJumping = false;
            }
        }
    }

    private void ApplyVisualHeight()
    {
        spriteTransform.localPosition = new Vector3(
            spriteTransform.localPosition.x,
            currentHeight,
            spriteTransform.localPosition.z
        );
    }

}
