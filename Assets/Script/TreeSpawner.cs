using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab; // Tree GameObject to spawn
    public int treeCount = 10; // Number of trees to spawn
    public Tilemap[] validTilemaps; // Tilemaps where trees are allowed to spawn
    public Tilemap[] invalidTilemaps; // Tilemaps to exclude from spawning
    public float minimumDistance = 2.0f; // Minimum distance between trees

    private List<Vector3> spawnedTreePositions = new List<Vector3>();

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        List<Vector3> potentialPositions = new List<Vector3>();

        // Gather all potential spawn positions from valid tilemaps
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

        // Shuffle the list of potential positions to ensure randomness
        ShuffleList(potentialPositions);

        int spawnedTrees = 0;

        foreach (Vector3 position in potentialPositions)
        {
            if (spawnedTrees >= treeCount)
                break;

            if (IsTooCloseToOtherTrees(position))
                continue;

            Instantiate(treePrefab, position, Quaternion.identity);
            spawnedTreePositions.Add(position);
            spawnedTrees++;
        }

      
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
