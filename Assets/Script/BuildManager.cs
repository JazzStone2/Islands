using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public float maxBuildDistance = 10f; // Maximum distance to allow building
    private Camera mainCamera; // Reference to the main camera
    private GameObject previewObject; // Temporary ghost object for previewing placement
    public List<GameObject> invalidTilemaps; // List of invalid tilemap GameObjects
    private List<Collider2D> invalidTilemapColliders = new List<Collider2D>(); // Colliders from invalid tilemaps
    private HashSet<Vector2Int> occupiedTiles = new HashSet<Vector2Int>(); // Tracks occupied tiles

    public float tileSize = 1f; // Size of a single tile

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera

        // Retrieve the colliders from the invalid tilemap GameObjects
        foreach (GameObject tilemap in invalidTilemaps)
        {
            if (tilemap != null)
            {
                Collider2D collider = tilemap.GetComponent<Collider2D>();
                if (collider != null)
                {
                    invalidTilemapColliders.Add(collider);
                }
                else
                {
                    Debug.LogWarning($"The GameObject '{tilemap.name}' does not have a Collider2D component!");
                }
            }
        }
    }

    void Update()
    {
        // Update the preview object position
        UpdatePreview();

        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBuilding();
        }
    }

    void UpdatePreview()
    {
        // Get the currently selected item from the inventory
        Item selectedItem = InventroyManager.instance.GetSelectedItem(false); // False ensures the item isn't deleted

        // Check if the selected item meets the criteria for building
        if (selectedItem != null && selectedItem.prefab != null && selectedItem.type == ItemType.BuildingBlock && selectedItem.actionType == ActionType.Place)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 snappedPosition = GetSnappedPosition(mousePosition);

            // Check if the click is within the max build distance
            if (Vector2.Distance(transform.position, snappedPosition) <= maxBuildDistance)
            {
                if (previewObject == null)
                {
                    // Create the preview object if it doesn't already exist
                    previewObject = Instantiate(selectedItem.prefab);
                    SetToPreviewMode(previewObject);
                }

                // Update the position of the preview object
                previewObject.transform.position = snappedPosition;

                // Check if placement is valid
                bool isPlacementValid = IsPlacementValid(snappedPosition);

                // Change color based on placement validity
                ChangePreviewColor(isPlacementValid ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 0f, 0f, 0.5f));
            }
        }
        else
        {
            // Destroy the preview object if the selected item is invalid or null
            if (previewObject != null)
            {
                Destroy(previewObject);
            }
        }
    }

    void SetToPreviewMode(GameObject preview)
    {
        // Customize the preview object to differentiate it visually
        Renderer renderer = preview.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(1f, 1f, 1f, 0.5f); // Make it semi-transparent
        }

        // Disable the collider while in preview mode
        Collider2D collider = preview.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Disable collider for preview
        }
    }

    bool IsPlacementValid(Vector3 position)
    {
        Vector2Int tilePosition = WorldToTilePosition(position);

        // Check if the tile is already occupied
        if (occupiedTiles.Contains(tilePosition))
        {
            return false; // Tile is occupied
        }

        // Check if the position overlaps any of the invalid colliders
        foreach (Collider2D collider in invalidTilemapColliders)
        {
            if (collider.OverlapPoint(position))
            {
                return false; // Invalid placement if the position overlaps an invalid collider
            }
        }

        return true; // Valid placement if no invalid colliders overlap and tile is free
    }

    void ChangePreviewColor(Color color)
    {
        // Change the color of the preview object
        Renderer renderer = previewObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    void PlaceBuilding()
    {
        // Get the currently selected item from the inventory
        Item selectedItem = InventroyManager.instance.GetSelectedItem(false); // False ensures the item isn't prematurely deleted

        // Check if the selected item meets the criteria for building
        if (selectedItem != null && selectedItem.prefab != null && selectedItem.type == ItemType.BuildingBlock && selectedItem.actionType == ActionType.Place)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 snappedPosition = GetSnappedPosition(mousePosition);
            Vector2Int tilePosition = WorldToTilePosition(snappedPosition);

            // Check if the click is within the max build distance
            if (Vector2.Distance(transform.position, snappedPosition) <= maxBuildDistance)
            {
                // Check if placement is valid
                if (IsPlacementValid(snappedPosition))
                {
                    // Instantiate the building prefab at the snapped position
                    GameObject placedObject = Instantiate(selectedItem.prefab, snappedPosition, Quaternion.identity);

                    // Enable the collider on the placed object
                    Collider2D collider = placedObject.GetComponent<Collider2D>();
                    if (collider != null)
                    {
                        collider.enabled = true; // Enable collider after placement
                    }

                    // Mark the tile as occupied
                    occupiedTiles.Add(tilePosition);

                    // Use the item (remove it from inventory)
                    InventroyManager.instance.GetSelectedItem(true);

                    // Destroy the preview object after placement
                    if (previewObject != null)
                    {
                        Destroy(previewObject);
                    }
                }
                else
                {
                    Debug.Log("Invalid placement area!");
                }
            }
        }
    }
    public bool IsTileOccupied(Vector2Int tilePosition)
{
    return occupiedTiles.Contains(tilePosition);
}


    Vector3 GetSnappedPosition(Vector2 position)
    {
        // Snap the position to the nearest tile
        float x = Mathf.Floor(position.x / tileSize) * tileSize;
        float y = Mathf.Floor(position.y / tileSize) * tileSize;
        return new Vector3(x, y, 0);
    }

    Vector2Int WorldToTilePosition(Vector3 position)
    {
        // Convert world position to tile position
        int x = Mathf.FloorToInt(position.x / tileSize);
        int y = Mathf.FloorToInt(position.y / tileSize);
        return new Vector2Int(x, y);
    }
}
