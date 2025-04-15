using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // Distance within which the zombie can attack
    public float attackInterval = 1f; // Time between attacks
    public int attackDamage = 10; // Amount of damage dealt to the player per attack

    private float attackTimer = 0f; // Timer to track attack intervals
    private Transform player; // Reference to the player's transform
    private PlayerHealth playerHealth; // Reference to the player's health script

    void Start()
    {
        // Automatically find the player if not assigned
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
        else
        {
        }
    }

    void Update()
    {
        if (player != null && playerHealth != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
        }
    }

    void AttackPlayer()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            // Deal damage to the player
            playerHealth.TakeDamage(attackDamage);

            // Reset attack timer
            attackTimer = 0f;

            // Optional: Add a sound or animation for the attack
        }
    }
}
