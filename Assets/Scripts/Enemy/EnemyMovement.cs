using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float stoppingDistance = 1f;
    
    private Sprite[] sprites;
    private SpriteRenderer sr;
    private float attackInterval = 2f;
    private bool isAttacking = false;
    private Vector3 cartesianPosition;
    private Vector2 facingDirection;
    private Transform playerTarget; 
    
    public void AssignSpeed(float speed) => moveSpeed = speed;
    public void AssignDamage(int dmg) => damage = dmg;
    public void AssignSprites(Sprite[] sprts) => sprites = sprts;

    public void Initialize(Vector3 startingCartesianPos, Transform target)
    {
        cartesianPosition = startingCartesianPos;
        playerTarget = target;
        
        UpdateVisualPosition();
    }

    private void Start()
    {
        sr =  gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (playerTarget == null) return;
        
        Vector3 targetCartesianPos = playerTarget.GetComponent<PlayerMovement>().GetWorldPosition();

        Vector3 direction = targetCartesianPos - cartesianPosition;
        direction.z = 0;

        if (direction.magnitude > stoppingDistance)
        {
            if (isAttacking)
            {
                isAttacking = false;
            }
            
            Vector3 movement = direction.normalized * (moveSpeed * Time.deltaTime);
            cartesianPosition += movement;
            
            if(movement.sqrMagnitude > 0.0001f)
                facingDirection = movement.normalized;
            
            UpdateVisualPosition();
            ApplyVisualRotation();
        }
        else
        {
            if(!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(StartAttack());
            }

            if (direction.sqrMagnitude > 0.0001f)
                facingDirection = direction.normalized;
            
            ApplyVisualRotation();
        }
    }

    private IEnumerator StartAttack()
    {
        while (isAttacking)
        {
            HealthBar.Instance.TakeDamage(damage);
            yield return new WaitForSeconds(attackInterval);
        }
    }
    
    private void ApplyVisualRotation()
    {
        if (sr == null) return;
        if (sprites == null) return;
        
        Vector2 dir = facingDirection;

        bool facingBack = dir.y > 0.1f;
        bool facingFront = dir.y < -0.1f;

        bool facingLeft = dir.x < -0.1f;
        bool facingRight = dir.x > 0.1f;

        int index = 0;
        
        if (facingFront)
        {
            if (facingLeft)
                index = 2;
            else if (facingRight)
                index = 0;
            else
                index = 0;
        }
        else if (facingBack)
        {
            if (facingLeft)
                index = 3;
            else if (facingRight)
                index = 1;
            else
                index = 1;
        }
        sr.sprite = sprites[index];
    }

    private void UpdateVisualPosition()
    {
        Vector3 isometricPosition = Utils.CartesianToIsometric(cartesianPosition);
        transform.position = new Vector3(
            isometricPosition.x, 
            isometricPosition.y, 
            0 
        );
    }
}
