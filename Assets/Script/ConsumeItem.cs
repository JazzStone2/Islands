using UnityEngine;
using Unity.VisualScripting;

public class ConsumableHandler : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the player's health script

    private void Start()
    {
        // Retrieve player health reference if not assigned
        if (playerHealth == null)
        {
            playerHealth = FindAnyObjectByType<PlayerHealth>();
        }
    }

    private void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0) && playerHealth.currentHealth != playerHealth.maxHealth)
        {
            UseSelectedItem();
        }
    }

    public void UseSelectedItem()
    {
        // Get the currently selected item from the inventory
        Item selectedItem = InventroyManager.instance.GetSelectedItem(false);

        // Check if the item exists and is consumable
        if (selectedItem != null && selectedItem.actionType == ActionType.Consume)
        {
            // Heal the player based on the item's health value
            playerHealth?.Heal(selectedItem.HeathGiven);

            

            // Remove or reduce the item count in the inventory
            bool isDeleted = InventroyManager.instance.GetSelectedItem(true);


        }
        else
        {

        }
    }
}
