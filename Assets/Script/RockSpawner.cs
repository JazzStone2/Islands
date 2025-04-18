using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RockSpawner : MonoBehaviour
{
    // Rock prefab to spawn
    public GameObject rockPrefab;

    // Tilemap to define spawnable areas
    public Tilemap tilemap;

    // Total number of rocks to spawn
    public int numberOfRocksToSpawn;

    // Spawn rocks on the tilemap
    public void SpawnRocks()
    {
        // Get all tile positions from the tilemap
        List<Vector3> availableTiles = new List<Vector3>();

        BoundsInt bounds = tilemap.cellBounds;

        // Iterate through each position within the tilemap's bounds
        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3Int cellPosition = new Vector3Int(pos.x, pos.y, pos.z);

            if (tilemap.HasTile(cellPosition))
            {
                // Convert the tilemap position to world position and add to the list
                Vector3 worldPosition = tilemap.CellToWorld(cellPosition) + tilemap.tileAnchor;
                availableTiles.Add(worldPosition);
            }
        }

        // Spawn rocks at random positions within the available tiles
        for (int i = 0; i < numberOfRocksToSpawn && availableTiles.Count > 0; i++)
        {
            // Choose a random index
            int randomIndex = Random.Range(0, availableTiles.Count);

            // Get the world position of the tile
            Vector3 spawnPosition = availableTiles[randomIndex];

            // Instantiate a rock at the tile position
            Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

            // Remove the used tile from the list to avoid duplicates
            availableTiles.RemoveAt(randomIndex);
        }
    }

    void Start()
    {
        // Call SpawnRocks at the start
        SpawnRocks();
    }
}
