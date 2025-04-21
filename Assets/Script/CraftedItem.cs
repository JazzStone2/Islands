using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Scriptable Objects/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public Item craftedItem; // The item to be crafted
    public Item[] requiredMaterials; // Array of required materials
    public int[] requiredAmounts; // Corresponding quantities for each material
    public int AmountGiven;
}
