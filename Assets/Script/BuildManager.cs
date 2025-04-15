using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public float maxBuildDistance = 10f; // Maximum distance to allow building
    private Camera mainCamera; // Reference to the main camera
    private GameObject previewObject; // Temporary ghost object for previewing placement
    public List<GameObject> invalidTilemaps; // List of invalid tilemap GameObjects

    private List<Collider2D> invalidTilemapColliders = new List<Collider2D>(); // Colliders from invalid tilemaps

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

            // Check if the click is within the max build distance
            if (Vector2.Distance(transform.position, mousePosition) <= maxBuildDistance)
            {
                if (previewObject == null)
                {
                    // Create the preview object if it doesn't already exist
                    previewObject = Instantiate(selectedItem.prefab);
                    SetToPreviewMode(previewObject);
                }

                // Update the position of the preview object
                previewObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

                // Check if placement is valid
                bool isPlacementValid = IsPlacementValid(previewObject.transform.position);

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
    }

    bool IsPlacementValid(Vector3 position)
    {
        // Check if the position overlaps any of the invalid colliders
        foreach (Collider2D collider in invalidTilemapColliders)
        {
            if (collider.OverlapPoint(position))
            {
                return false; // Invalid placement if the position overlaps an invalid collider
            }
        }
        return true; // Valid placement if no invalid colliders overlap
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

            // Check if the click is within the max build distance
            if (Vector2.Distance(transform.position, mousePosition) <= maxBuildDistance)
            {
                // Check if placement is valid
                if (IsPlacementValid(mousePosition))
                {
                    // Instantiate the building prefab at the mouse position
                    Instantiate(selectedItem.prefab, new Vector3(mousePosition.x, mousePosition.y, 0), Quaternion.identity);

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
}
