using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterFishing : MonoBehaviour
{
    public GameObject fishingUI; // Reference to the fishing UI GameObject
    public Tilemap waterTilemap; // Reference to the water tilemap
    public float maxDistance = 2.0f; // Maximum distance the player can be from the water tile

    private void Start()
    {
        // Hide the fishing UI at the start
        if (fishingUI != null)
        {
            fishingUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Detect left mouse button click
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedTilePosition = waterTilemap.WorldToCell(mousePosition);

            // Check if the clicked tile belongs to the water tilemap
            if (waterTilemap.HasTile(clickedTilePosition))
            {
                // Get the center position of the clicked tile
                Vector3 tileWorldPosition = waterTilemap.GetCellCenterWorld(clickedTilePosition);

                // Check if the player is close enough to the clicked tile
                if (Vector3.Distance(tileWorldPosition, transform.position) <= maxDistance)
                {
                    // Check if the player has the fishing rod equipped
                    Item selectedItem = InventroyManager.instance.GetSelectedItem(false);
                    if (selectedItem != null && selectedItem.type == ItemType.FishingRod && selectedItem.actionType == ActionType.Fishing)
                    {
                        OpenFishingUI();
                    }
                }
                else
                {
                    Debug.Log("You are too far from the fishing spot!");
                }
            }
        }
    }

    private void OpenFishingUI()
    {
        // Open the fishing UI
        if (fishingUI != null)
        {
            fishingUI.SetActive(true);
            Debug.Log("Fishing UI opened.");
        }
        else
        {
            Debug.LogWarning("Fishing UI reference is missing!");
        }
    }
}
