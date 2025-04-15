using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]

    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Weapon Properties")]
    public int damage = 0; // Damage dealt by the item (used if the item is a weapon)

    [Header("Consumable Health Point")]
    public int HeathGiven = 0;

    [Header("ONLY UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;

    [Header("Building Properties")]
    public GameObject prefab; // Prefab to be instantiated for building items
}

public enum ItemType
{
    BuildingBlock,
    Tool,
    Food,
    Weapon,
    FishingRod,
    Axe,
    CraftMaterial,
    Shovel
}

public enum ActionType
{
    Break,
    Place,
    Fishing,
    Attack,
    Consume,
    CraftMaterial
}
