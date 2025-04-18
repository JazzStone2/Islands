using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public InventroyManager inventoryManager; // Reference to the InventoryManager
    public CraftingRecipe[] craftingRecipes; // Array of all available recipes
    public Button[] craftButtons; // Array of buttons for crafting

private void Start()
{
    foreach (Button button in craftButtons)
    {
        button.onClick.RemoveAllListeners();
    }

    for (int i = 0; i < craftButtons.Length; i++)
    {
        int index = i;
        craftButtons[i].onClick.AddListener(() => CraftItem(index));
    }

    Debug.Log("Crafting buttons initialized");
}




private bool isCrafting = false;

public void CraftItem(int recipeIndex)
{
    if (isCrafting) return; // Block execution if already crafting

    isCrafting = true; // Start crafting process
    CraftingRecipe recipe = craftingRecipes[recipeIndex];

    if (HasRequiredMaterials(recipe))
    {
        for (int i = 0; i < recipe.requiredMaterials.Length; i++)
        {
            inventoryManager.RemoveItem(recipe.requiredMaterials[i], recipe.requiredAmounts[i]);
        }

        inventoryManager.AddItem(recipe.craftedItem);
        Debug.Log($"Crafted {recipe.craftedItem.name} successfully!");
    }
    else
    {
        Debug.Log($"Not enough materials to craft {recipe.craftedItem.name}.");
    }

    // Reset crafting flag after a short delay
    StartCoroutine(ResetCraftingFlag());
}

private IEnumerator ResetCraftingFlag()
{
    yield return new WaitForSeconds(0.1f); // Small delay
    isCrafting = false;
}


    private bool HasRequiredMaterials(CraftingRecipe recipe)
    {
        // Check if the inventory has all required materials for the recipe
        for (int i = 0; i < recipe.requiredMaterials.Length; i++)
        {
            if (!inventoryManager.HasItem(recipe.requiredMaterials[i], recipe.requiredAmounts[i]))
            {
                return false;
            }
        }
        return true;
    }
}
