using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab; // Assign your fish prefab in the Inspector
    public RectTransform targetImage; // Assign your target UI image in the Inspector
    public GameObject uiPanel; // The parent UI GameObject to check if active
    public int maxFish = 2; // Maximum number of fish allowed at a time
    public float spawnInterval = 2f; // Time between spawns in seconds

    private float spawnTimer;
    private List<GameObject> spawnedFish = new List<GameObject>(); // Keep track of spawned fish

    void Update()
    {
        // Check if the UI panel is active
        if (uiPanel.activeSelf)
        {
            // Spawn fish at intervals if below maxFish limit
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval && spawnedFish.Count < maxFish)
            {
                spawnTimer = 0f;
                SpawnFish();
            }
        }
        else
        {
            // If UI is not active, clear all spawned fish
            ClearFish();
        }
    }

    void SpawnFish()
    {
        // Get the world position of the image (behind it)
        Vector3 imagePosition = targetImage.transform.position;

        // Adjust position to spawn behind the image
        Vector3 spawnPosition = imagePosition;
        spawnPosition.z -= 1f; // Spawning behind (adjust z-axis)

        // Randomize x and y positions within the bounds of the image
        Rect rect = targetImage.rect;
        spawnPosition.x += Random.Range(-rect.width / 2, rect.width / 2);
        spawnPosition.y += Random.Range(-rect.height / 2, rect.height / 2);

        // Instantiate the fish prefab at the calculated position
        GameObject fish = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);

        // Add the spawned fish to the list
        spawnedFish.Add(fish);
    }

    void ClearFish()
    {
        // Destroy all fish and clear the list
        foreach (GameObject fish in spawnedFish)
        {
            Destroy(fish);
        }
        spawnedFish.Clear();
    }
}
