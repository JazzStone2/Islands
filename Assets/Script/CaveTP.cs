using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveTP : MonoBehaviour
{
    public GameObject targetObject; // Reference to the target GameObject where the player will be teleported
    public Tilemap CaveTilemap; 
    public float maxDistance = 2.0f; // Maximum distance the player can be from the water tile

    private void Update()
    {
        Item selectedItem = InventroyManager.instance.GetSelectedItem(false);
        // Detect left mouse button click
        if (Input.GetMouseButtonDown(0) && selectedItem.actionType == ActionType.Climbing) // Left mouse button click
        {
            
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedTilePosition = CaveTilemap.WorldToCell(mousePosition);

            // Check if the clicked tile belongs to the water tilemap
            if (CaveTilemap.HasTile(clickedTilePosition))
            {
                // Get the center position of the clicked tile
                Vector3 tileWorldPosition = CaveTilemap.GetCellCenterWorld(clickedTilePosition);

                // Check if the player is close enough to the clicked tile
                if (Vector3.Distance(tileWorldPosition, transform.position) <= maxDistance)
                {
                    // Teleport the player to the target GameObject
                    if (targetObject != null)
                    {
                        transform.position = targetObject.transform.position;
                        Debug.Log("Player teleported to the target object.");
                    }
                    else
                    {
                        Debug.LogWarning("Target object reference is missing!");
                    }
                }
                else
                {
                    Debug.Log("You are too far from the teleportation spot!");
                }
            }
        }
    }
}
