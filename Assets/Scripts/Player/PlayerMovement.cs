using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float normalFriction = 0.8f; 
    
    [Header("State Settings")]
     public bool isDizzy = false;
     public bool isDrunk = false;
     [SerializeField] private float dizzyFriction = 0.95f;
     [SerializeField] private float drunkFriction = 0.98f;
     [SerializeField] private float dizzyAcceleration = 3f;
     [SerializeField] private float drunkAcceleration = 1.5f;
     [SerializeField] private float dizzyInversionChance = 0.2f;
     [SerializeField] private float drunkInversionChance = 0.5f;
    
    [Header("Reference")]
    [SerializeField] private Transform spriteTransform;
    
    public Vector2 CartesianFacingDirection { get; private set; } = Vector2.up; 
    
    //private bool isJumping = false;
    private float verticalVelocity = 0f;
    private float currentHeight = 0f;  
    private Vector3 worldPosition; 
    
    private float hvX = 0f; 
    private float hvY = 0f; 
    
    public Vector3 GetWorldPosition() => worldPosition; 

    void Start()
    {
        worldPosition = transform.position;
    }
    void Update()
    {
        if (this != null)
        {
            InputMovement();
            HandleAimingRotation();
            HandleJump();
            UpdateVisualPosition();
            
            if (spriteTransform == null)
            {
                Debug.Log("Sprite not assigned.");
                return;
            }
            
            ApplyVisualHeight();
            ApplyVisualRotation();
        }
    }
    
    private void InputMovement()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        
        float accelerationMultiplier;
        float currentFriction; 

        if (isDrunk)
        {
            if (UnityEngine.Random.value < drunkInversionChance) hInput *= -1;
            if (UnityEngine.Random.value < drunkInversionChance) vInput *= -1;
            
            hvX += (UnityEngine.Random.Range(-0.5f, 0.5f) * Time.deltaTime);
            hvY += (UnityEngine.Random.Range(-0.5f, 0.5f) * Time.deltaTime);
            
            accelerationMultiplier = drunkAcceleration;
            currentFriction = drunkFriction;
        }
        else if (isDizzy)
        {
            if (UnityEngine.Random.value < dizzyInversionChance) hInput *= -1;
            if (UnityEngine.Random.value < dizzyInversionChance) vInput *= -1;
            
            accelerationMultiplier = dizzyAcceleration;
            currentFriction = dizzyFriction;
        }
        else
        {
            accelerationMultiplier = 5f;
            currentFriction = normalFriction;
        }
    
        Vector3 force = new Vector3(vInput, -hInput, 0); 
        //Vector3 force = new Vector3(hInput, vInput, 0); 
        if(force.sqrMagnitude > 1) force.Normalize();
        
        float accelerationFactor = moveSpeed * accelerationMultiplier; 
        hvX += force.x * (accelerationFactor * Time.deltaTime);
        hvY += force.y * (accelerationFactor * Time.deltaTime); 
        
        if (force.sqrMagnitude < 0.01f)
        {
            hvX *= currentFriction; 
            hvY *= currentFriction;
        }
        
        Vector2 currentHV = new Vector2(hvX, hvY);
        if (currentHV.sqrMagnitude > moveSpeed * moveSpeed)
        {
            currentHV = currentHV.normalized * moveSpeed;
            hvX = currentHV.x;
            hvY = currentHV.y;
        }

        worldPosition.x += hvX * Time.deltaTime;
        worldPosition.y += hvY * Time.deltaTime;
        
        //transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
    }
    
    private void HandleAimingRotation()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldTarget = ScreenToCartesian(mouseScreenPos);
        
        Vector3 direction = (mouseWorldTarget - worldPosition);
        
        Vector2 flatDirection = new Vector2(direction.x, direction.y).normalized;

        if (flatDirection.sqrMagnitude > 0.01f)
        {
            CartesianFacingDirection = flatDirection;
        }
    }
    
    private Vector3 ScreenToCartesian(Vector3 screenPos)
    {
        if (Camera.main == null) return worldPosition;
        
        Vector3 isoWorldPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, transform.position.z)
        );
        
        float isoX = isoWorldPoint.x;
        float isoY = isoWorldPoint.y;

        Vector3 cartesianTarget = new Vector3(0, 0, worldPosition.z)
        {
            x = (isoX * 0.5f) + isoY, 
            y = isoY - (isoX * 0.5f)
        };
        
        return cartesianTarget;
    }
    
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentHeight == 0)
        {
            verticalVelocity = jumpForce;
            //isJumping = true;
        }

        if (currentHeight > 0 || verticalVelocity > 0)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            currentHeight += verticalVelocity * Time.deltaTime;

            if (currentHeight < 0)
            {
                currentHeight = 0;
                verticalVelocity = 0;
                //isJumping = false;
            }
        }
    }
    
    private void UpdateVisualPosition()
    {
        Vector3 isometricPosition =  IsometricUtil.CartesianToIsometric(worldPosition);
        
        transform.position = new Vector3(isometricPosition.x, isometricPosition.y, worldPosition.z);
    }
    
    private void ApplyVisualRotation()
    {
        if (spriteTransform == null) return;
        
        if (CartesianFacingDirection.x < -0.1f)
        {
            spriteTransform.localScale = new Vector3(-2, 2, 2);
        }
        else if (CartesianFacingDirection.x > 0.1f)
        {
            spriteTransform.localScale = new Vector3(2, 2, 2);
        }
    }

    private void ApplyVisualHeight()
    {
        if (spriteTransform == null)
        {
            Debug.Log("Sprite transform not assigned for visual height.");
            return;
        }
        
        spriteTransform.localPosition = new Vector3(
            spriteTransform.localPosition.x,
            currentHeight,
            spriteTransform.localPosition.z
        );
    }

    public void SetPlayerState(bool drunk, bool dizzy)
    {
        if (drunk)
        {
            isDrunk = true;
            isDizzy = false;
        }
        else if (dizzy)
        {
            isDrunk = false;
            isDizzy = true;
        }
        else
        {
            isDrunk = false;
            isDizzy = false;
        }
    }
}
