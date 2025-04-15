using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f; // Range within which the player can attack

    private void Update()
    {
        // Check if the attack button is pressed
        if (Input.GetButtonDown("Fire1")) // Default attack input
        {
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        // Get the currently selected item from the inventory
        Item selectedItem = InventroyManager.instance.GetSelectedItem(false);

        if (selectedItem != null && selectedItem.type == ItemType.Weapon && selectedItem.actionType == ActionType.Attack)
        {
            int damage = selectedItem.damage; // Use the weapon's damage value
            // Iterate through all enemy objects and check distance
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= attackRange)
                {
                    // Apply damage to the enemy's health component
                    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damage);
                    }
                    else
                    {
            
                    }
                }
            }
        }
        else
        {

        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
