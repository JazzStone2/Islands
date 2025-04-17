using UnityEngine;

public class TorchPickup : MonoBehaviour
{
    public GameObject spawnPrefab; // Prefab to spawn
    public float pickupRadius = 2f; // Radius for player interaction
    private Transform playerTransform; // Reference to player's transform
    private BuildManager buildManager; // Reference to BuildManager

    private void Start()
    {
        // Find player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }

        // Find the BuildManager in the scene
        buildManager = FindAnyObjectByType<BuildManager>();
        if (buildManager == null)
        {
            Debug.LogWarning("BuildManager not found in the scene!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Check raycast hits and proximity
            if (hit.collider != null && hit.collider.gameObject == gameObject &&
                playerTransform != null && Vector3.Distance(playerTransform.position, transform.position) <= pickupRadius)
            {
                PickUp();
            }
        }
    }

    private void PickUp()
    {
        if (spawnPrefab != null)
        {
            Instantiate(spawnPrefab, transform.position, transform.rotation);

            if (buildManager != null)
            {
                // Free the tile where the torch was picked up
                Vector2Int tilePosition = buildManager.WorldToTilePosition(transform.position);
                if (buildManager.IsTileOccupied(tilePosition))
                {
                    buildManager.occupiedTiles.Remove(tilePosition);
                    Debug.Log($"Tile at {tilePosition} is now free.");
                }
            }

            Destroy(gameObject); // Remove the torch
        }
        else
        {
            Debug.LogWarning("SpawnPrefab is not assigned!");
        }
    }
}
