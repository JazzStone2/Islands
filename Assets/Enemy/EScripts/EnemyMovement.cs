using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player; // Now private to this script
    public float followRange = 10f; // Distance within which the zombie follows the player
    public float speed = 2f; // Speed of the zombie
    public float wanderSpeed = 1f; // Speed of aimless wandering
    public float wanderChangeInterval = 2f; // Time between direction changes

    private Rigidbody2D rb;
    private Vector2 wanderDirection;
    private float wanderTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Automatically find the player using its tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        wanderDirection = Random.insideUnitCircle.normalized; // Start with a random direction
    }


    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= followRange)
            {
                FollowPlayer();
            }
            else
            {
                WanderAimlessly();
            }
        }
        CheckFlip();
    }

    void FollowPlayer()
    {
        if (rb != null && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
    }

    void WanderAimlessly()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderChangeInterval)
        {
            wanderDirection = Random.insideUnitCircle.normalized;
            wanderTimer = 0f;
        }

        rb.linearVelocity = wanderDirection * wanderSpeed;
    }

    // Optional: Method to assign player reference manually
    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

public void CheckFlip()
{
    if (rb.linearVelocity.x < -0.1f)
    {
        if (transform.localScale.x > 0) // Ensure we only flip once
        {
            FlipLogic();
        }
    }
    else if (rb.linearVelocity.x > 0.1f)
    {
        if (transform.localScale.x < 0) // Ensure we only flip once
        {
            FlipLogic();
        }
    }
}

public void FlipLogic()
{
    Vector3 newScale = transform.localScale;
    newScale.x = -newScale.x;
    transform.localScale = newScale;
}

}
