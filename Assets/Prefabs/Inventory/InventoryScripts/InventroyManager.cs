using Unity.VisualScripting; // Reference to Unity Visual Scripting library
using UnityEngine; // Unity engine namespace

public class InventroyManager : MonoBehaviour
{
    // Singleton instance of InventoryManager to ensure a single active instance
    public static InventroyManager instance;
    public Item[] startItems; // Array to hold initial items in the inventory

    public int maxItems = 50; // Maximum stack size for items
    public InventorySlot[] inventorySlots; // Array of slots to store items
    public GameObject inventoryItemPrefab; // Prefab for inventory items
    int selectedSlot = -1; // Index of the currently selected inventory slot

    private void Awake()
    {
        // Initialize the singleton instance
        instance = this;
    }

    private void Start()
    {
        // Select the first inventory slot by default
        ChangeSelectedSlot(0);
        // Add initial items to the inventory
        foreach (var item in startItems)
        {
            AddItem(item);
        }
    }

    private void Update()
    {
        // Check for user input to select inventory slots using number keys
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= 6) // Restrict to 6 slots
            {
                ChangeSelectedSlot(number - 1); // Convert to zero-based index
            }
        }

        // Handle mouse scroll wheel input for hotbar slot selection
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) // Check if there is scrolling action
        {
            int newValue = selectedSlot + (scroll > 0 ? -1 : 1); // Scroll up decreases slot, scroll down increases
            if (newValue < 0) // Wrap around to the last slot if scrolling up past the first slot
            {
                newValue = 5; // Restrict to 0-5 index for hotbar (six slots)
            }
            else if (newValue > 5) // Wrap around to the first slot if scrolling down past the last slot
            {
                newValue = 0;
            }
            ChangeSelectedSlot(newValue); // Update the selected slot
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        // Deselect the previously selected slot
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselected();
        }

        // Select the new slot
        inventorySlots[newValue].Selected();
        selectedSlot = newValue; // Update the current selected slot index
    }

    public bool AddItem(Item item)
    {
        // Check if the item can be stacked in an existing slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxItems && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++; // Increase the item count in the stack
                itemInSlot.ResfreshCount(); // Update the UI for the item count
                return true;
            }
        }

        // Add the item to a new slot if stacking isn't possible
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot); // Spawn a new item instance in the slot
                return true;
            }
        }
        return false; // Return false if the inventory is full
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        // Instantiate a new inventory item in the specified slot
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item); // Initialize the item with its properties
    }

    public Item GetSelectedItem(bool use)
    {
        // Get the item in the currently selected slot
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null) // Check if there is an item in the slot
        {
            Item item = itemInSlot.item;
            if (use) // Reduce the count or remove the item if "use" is true
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject); // Remove the item from the inventory if count reaches zero
                }
                else
                {
                    itemInSlot.ResfreshCount(); // Update the UI for the item count
                }
            }
            return item; // Return the item
        }
        return null; // Return null if no item is found in the slot
    }
    public bool HasItem(Item item, int requiredAmount)
{
    int itemCount = 0;
    foreach (var slot in inventorySlots)
    {
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null && itemInSlot.item == item)
        {
            itemCount += itemInSlot.count;
        }
        if (itemCount >= requiredAmount)
        {
            return true;
        }
    }
    return false;
}

public void RemoveItem(Item item, int quantityToRemove)
{
    foreach (var slot in inventorySlots)
    {
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null && itemInSlot.item == item)
        {
            // Calculate how many items to remove from this slot
            int removeAmount = Mathf.Min(itemInSlot.count, quantityToRemove);
            itemInSlot.count -= removeAmount; // Deduct from the slot count
            quantityToRemove -= removeAmount; // Reduce the remaining quantity to remove

            if (itemInSlot.count <= 0)
            {
                Destroy(itemInSlot.gameObject); // Remove the inventory item if count is zero
            }
            else
            {
                itemInSlot.ResfreshCount(); // Update the count display in the UI
            }

            if (quantityToRemove <= 0) // Exit early if all required items have been removed
            {
                return;
            }
        }
    }
}


}
