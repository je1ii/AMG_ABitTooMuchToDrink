using UnityEngine;

public class BeerProjectile : MonoBehaviour
{
    public Vector3 velocity;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float groundY = 0f;  
    [SerializeField] private float lifeAfterStop = 0.5f;

    private bool landed = false;
    private float timer = 0f;

    void Update()
    {
        if (landed)
        {
            timer += Time.deltaTime;
            if (timer >= lifeAfterStop)
                Destroy(gameObject);
            return;
        }

        transform.position += velocity * Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;

        if (transform.position.y <= groundY)
        {
            Vector3 p = transform.position;
            p.y = groundY;
            transform.position = p;

            velocity = Vector3.zero;
            landed = true;
        }
    }
}
