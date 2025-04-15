using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System; // Required for TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the player
    public int currentHealth;

    public Slider healthSlider; // Reference to a UI slider for the health bar
    public TMP_Text healthText; // Reference to a TextMeshPro UI text for displaying health
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        animator = GetComponent<Animator>();
        // Update the UI on start
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Subtract damage from health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health is within bounds

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount; // Add healing amount to health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't exceed max

        UpdateHealthUI();
        PlayHealthUpAnimation(); // Play the healing animation
    }

    void UpdateHealthUI()
    {
        // Update the health bar slider
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }

        // Update the health text with TextMeshPro
        if (healthText != null)
        {
            healthText.text = $" {currentHealth}%";
        }
    }

    void PlayHealthUpAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("HealthUp", true); // Enable HealthUp animator bool

            // Wait for the animation to finish and then reset the bool
            StartCoroutine(ResetHealthUpAnimation());
        }
    }

    System.Collections.IEnumerator ResetHealthUpAnimation()
    {
        if (animator != null)
        {
            // Get the length of the HealthUp animation clip
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (!stateInfo.IsName("HealthUp"))
            {
                yield return null; // Wait for the animation to start
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            yield return new WaitForSeconds(stateInfo.length); // Wait for the animation to finish
            animator.SetBool("HealthUp", false); // Reset HealthUp animator bool
        }
    }

    void Die()
    {
        // Trigger a death animation here if needed
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Time.timeScale = 0; // Pause the game
    }
}
