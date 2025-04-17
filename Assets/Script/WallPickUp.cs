using UnityEngine;

public class WallScript : MonoBehaviour
{
    public GameObject debrisPrefab;
    public float hitsToBreak = 5f; // Number of hits required to break the wall
    public float interactionRange = 3f; // Range within which the player can interact

    private float currentHits = 0f;
    private Transform playerTransform;

    // Reference to the BuildManager
    private BuildManager buildManager;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;

        // Find and reference the BuildManager in the scene
        buildManager = FindAnyObjectByType<BuildManager>();
        if (buildManager == null)
        {
            Debug.LogError("BuildManager not found in the scene!");
        }
    }

    private void OnMouseDown()
    {
        if (IsPlayerNearby())
        {
            Item selectedItem = InventroyManager.instance.GetSelectedItem(false);

            // Check if the selected item is an axe
            if (selectedItem != null && selectedItem.type == ItemType.Axe && selectedItem.actionType == ActionType.Break)
            {
                currentHits++;

                // Check if the wall has received enough hits
                if (currentHits >= hitsToBreak)
                {
                    int dropCount = 1; // Randomize the number of dropped items

                    for (int i = 0; i < dropCount; i++)
                    {
                        Instantiate(debrisPrefab, transform.position, Quaternion.identity);
                    }

                    // Mark the tile as valid after destruction
                    Vector2Int tilePosition = buildManager.WorldToTilePosition(transform.position);
                    if (buildManager != null)
                    {
                        buildManager.occupiedTiles.Remove(tilePosition);
                    }

                    // Destroy the wall
                    Destroy(gameObject);
                }
            }
        }
    }

    private bool IsPlayerNearby()
    {
        return Vector3.Distance(playerTransform.position, transform.position) <= interactionRange;
    }
}
