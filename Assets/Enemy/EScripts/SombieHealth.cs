using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy
    public int currentHealth;

    [Header("Damage Effects")]
    public GameObject damageEffectPrefab; // Effect shown when enemy takes damage

    [Header("Death Settings")]
    public GameObject deathEffectPrefab; // Particle effect shown on death
    public GameObject lootPrefab; // Optional loot dropped upon death

    [Header("Daytime Health Settings")]
    public bool loseHealthAtDay = true; // Toggle for losing health during daytime
    public float healthLossRate = 1f; // Health loss per second during daytime

    private DayAndNight dayAndNight; // Reference to the DayAndNight script
    private Animator animator; // Reference to Animator component

    void Start()
    {
        currentHealth = maxHealth; // Initialize health

        // Locate the DayAndNight script in the scene
        dayAndNight = FindAnyObjectByType<DayAndNight>();
        if (dayAndNight == null)
        {
            Debug.LogError("DayAndNight script not found! Ensure it is added to a GameObject in the scene.");
        }

        // Get the Animator component attached to the enemy
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found! Ensure an Animator is attached to the enemy GameObject.");
        }
    }

    void Update()
    {
        if (loseHealthAtDay)
        {
            LoseHealthDuringDay();
        }
        CheckHealthStatus();
    }

    private void LoseHealthDuringDay()
    {
        if (dayAndNight != null && dayAndNight.isDaytime) // Check if it's daytime
        {
            // Reduce health over time during daytime
            currentHealth -= Mathf.RoundToInt(healthLossRate * Time.deltaTime);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health stays within 0 and maxHealth
        }
    }

    private void CheckHealthStatus()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Subtract damage from health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Trigger the Hit animation
        if (animator != null)
        {
            animator.SetBool("Hit", true);
        }

        // Reset the Hit boolean after a short delay (optional)
        Invoke("ResetHitAnimation", 0.1f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ResetHitAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("Hit", false);
        }
    }

    void Die()
    {
        // Start the coroutine to handle the death process
        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        // Trigger the "Death" animation
        if (animator != null)
        {
            animator.SetTrigger("Dead");

            // Wait until the current state is finished
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (stateInfo.IsName("Dead") && stateInfo.normalizedTime < 1.0f)
            {
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }
        }



        // Drop loot if specified
        if (lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Destroy the enemy GameObject
    }
}
