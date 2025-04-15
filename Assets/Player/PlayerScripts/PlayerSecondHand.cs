using UnityEngine;

public class SecondhandItemManager : MonoBehaviour
{
    public InventroyManager inventoryManager; // Reference to the main InventoryManager
    public int secondhandSlotIndex = 1; // Index for the secondhand slot (default is 1)

    private void Start()
    {
        if (inventoryManager == null)
        {
        }
    }

    private void Update()
    {
        // Check for right-click input to use the item in the secondhand slot
        if (Input.GetMouseButtonDown(1)) // 1 is the right mouse button
        {
            UseSecondhandItem();
        }
    }

    public void UseSecondhandItem()
    {
        if (inventoryManager == null)
        {
            
            return;
        }

        // Ensure the secondhandSlotIndex is valid
        if (secondhandSlotIndex < 0 || secondhandSlotIndex >= inventoryManager.inventorySlots.Length)
        {
            
            return;
        }

        // Access the slot based on the chosen secondhandSlotIndex
        InventorySlot secondhandSlot = inventoryManager.inventorySlots[secondhandSlotIndex];

        // Check if there's an item in the selected secondhand slot
        InventoryItem itemInSlot = secondhandSlot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;

            // Perform the appropriate action based on the ActionType
            switch (item.actionType)
            {
                case ActionType.Consume:
                    ConsumeItem(item, itemInSlot);
                    break;
                case ActionType.Place:
                    PlaceItem(item);
                    break;
                case ActionType.Attack:
                    AttackWithItem(item);
                    break;
                case ActionType.Fishing:
                    FishWithItem(item);
                    break;
                case ActionType.Break:
                    BreakItem(item);
                    break;
                case ActionType.CraftMaterial:
                    Debug.Log($"Using crafting material: {item.name}");
                    break;
                default:
                    Debug.Log($"Unhandled action for item: {item.name}");
                    break;
            }
        }
        else
        {

        }
    }

private void ConsumeItem(Item item, InventoryItem itemInSlot)
{
    // Apply health restoration logic here
    PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>(); // Example health script
    if (playerHealth != null)
    {
        // Check if the player's health is not already at maximum
        if (playerHealth.currentHealth <= playerHealth.maxHealth - 1)
        {
            playerHealth.Heal(item.HeathGiven);

            // Reduce the item count and remove it if fully used
            itemInSlot.count--;
            if (itemInSlot.count <= 0)
            {
                Destroy(itemInSlot.gameObject);
            }
            else
            {
                itemInSlot.ResfreshCount();
            }
        }
        else
        {
            Debug.Log("Player's health is already at maximum. Item not consumed.");
        }
    }
}


    private void PlaceItem(Item item)
    {
        Debug.Log($"Placed item {item.name} in range {item.range}.");
        // Example logic for placing an item using the item's properties
    }

    private void AttackWithItem(Item item)
    {
        Debug.Log($"Attacked with {item.name}, dealing {item.damage} damage.");
        // Example attack logic (e.g., deal damage to an enemy)
    }

    private void FishWithItem(Item item)
    {
        Debug.Log($"Fishing with {item.name}...");
        // Add fishing logic here
    }

    private void BreakItem(Item item)
    {
        Debug.Log($"Breaking something with {item.name}.");
        // Add logic for breaking objects here
    }
}
