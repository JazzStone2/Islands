using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    public GameObject leafPrefab; // Assign your leaf prefab in the Inspector
    public Transform treePosition; // Position above the tree where leaves spawn
    public float spawnInterval = 2.0f; // Time between leaf spawns

    private void Start()
    {
        // Start spawning leaves at regular intervals
        InvokeRepeating(nameof(SpawnLeaf), spawnInterval, spawnInterval);
    }

    private void SpawnLeaf()
    {
        // Instantiate the leaf prefab at the tree position with no rotation
        Instantiate(leafPrefab, treePosition.position, Quaternion.identity);
    }
}
