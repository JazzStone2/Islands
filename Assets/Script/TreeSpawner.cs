using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab; // Tree GameObject to spawn
    public int treeCount = 10; // Total number of trees to spawn
    public int clusterCount = 3; // Number of tree clusters
    public int treesPerCluster = 5; // Number of trees per cluster
    public float clusterRadius = 3.0f; // Radius of each cluster
    public Tilemap[] validTilemaps; // Tilemaps where trees are allowed to spawn
    public Tilemap[] invalidTilemaps; // Tilemaps to exclude or keep distance from
    public float minimumDistance = 2.0f; // Minimum distance between trees
    public float safeDistanceFromInvalidTilemap = 3.0f; // Safe distance from invalid tilemaps

    private List<Vector3> spawnedTreePositions = new List<Vector3>();

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        List<Vector3> potentialPositions = GatherPotentialPositions();

        // Shuffle the list for randomness
        ShuffleList(potentialPositions);

        int spawnedTrees = 0;

        // Spawn clusters
        for (int i = 0; i < clusterCount; i++)
        {
            if (potentialPositions.Count == 0 || spawnedTrees >= treeCount)
                break;

            Vector3 clusterCenter = potentialPositions[0];
            potentialPositions.RemoveAt(0); // Reserve cluster center

            for (int j = 0; j < treesPerCluster && spawnedTrees < treeCount; j++)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-clusterRadius, clusterRadius),
                    Random.Range(-clusterRadius, clusterRadius),
                    0);

                Vector3 treePosition = clusterCenter + randomOffset;

                if (IsTooCloseToOtherTrees(treePosition) || IsTooCloseToInvalidTilemap(treePosition))
                    continue;

                Instantiate(treePrefab, treePosition, Quaternion.identity);
                spawnedTreePositions.Add(treePosition);
                spawnedTrees++;
            }
        }

        // Spawn remaining trees randomly
        foreach (Vector3 position in potentialPositions)
        {
            if (spawnedTrees >= treeCount)
                break;

            if (IsTooCloseToOtherTrees(position) || IsTooCloseToInvalidTilemap(position))
                continue;

            Instantiate(treePrefab, position, Quaternion.identity);
            spawnedTreePositions.Add(position);
            spawnedTrees++;
        }
    }

    List<Vector3> GatherPotentialPositions()
    {
        List<Vector3> potentialPositions = new List<Vector3>();

        foreach (Tilemap validTilemap in validTilemaps)
        {
            BoundsInt bounds = validTilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    Vector3 worldPosition = validTilemap.CellToWorld(cellPosition) + validTilemap.tileAnchor;

                    if (validTilemap.GetTile(cellPosition) != null && !IsTileOnInvalidTilemap(cellPosition))
                    {
                        potentialPositions.Add(worldPosition);
                    }
                }
            }
        }
        return potentialPositions;
    }

    bool IsTileOnInvalidTilemap(Vector3Int tilePosition)
    {
        foreach (Tilemap tilemap in invalidTilemaps)
        {
            if (tilemap.HasTile(tilePosition))
            {
                return true;
            }
        }
        return false;
    }

    bool IsTooCloseToInvalidTilemap(Vector3 position)
    {
        foreach (Tilemap tilemap in invalidTilemaps)
        {
            BoundsInt bounds = tilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    Vector3 worldPosition = tilemap.CellToWorld(cellPosition) + tilemap.tileAnchor;

                    if (tilemap.GetTile(cellPosition) != null &&
                        Vector3.Distance(worldPosition, position) < safeDistanceFromInvalidTilemap)
                    {
                        return true; // Too close to an invalid tile
                    }
                }
            }
        }
        return false;
    }

    bool IsTooCloseToOtherTrees(Vector3 position)
    {
        foreach (Vector3 existingTreePosition in spawnedTreePositions)
        {
            if (Vector3.Distance(existingTreePosition, position) < minimumDistance)
            {
                return true; // Too close to an existing tree
            }
        }
        return false;
    }

    void ShuffleList(List<Vector3> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            Vector3 temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
