using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Prefab of the zombie to spawn
    public int maxZombies = 25; // Maximum number of zombies at once
    public float spawnRadius = 10f; // Distance from the camera to spawn zombies
    public float spawnInterval = 1f; // Time interval between spawns

    private float spawnTimer = 0f; // Timer to track spawning
    private int currentZombieCount = 0; // Keeps track of active zombies
    private DayAndNight dayAndNightScript; // Reference to your day-night script

    void Start()
    {
        // Find the DayAndNight script in the scene
        dayAndNightScript = FindAnyObjectByType<DayAndNight>();
        if (dayAndNightScript == null)
        {
            Debug.LogError("No DayAndNight script found! Please ensure it's added to the scene.");
        }
    }

    void Update()
    {
        // Check if it's night (you can adjust the threshold as needed)
        if (dayAndNightScript != null && dayAndNightScript.GetNightFactor() > 0.5f)
        {
            // Spawn zombies if under the limit
            if (currentZombieCount < maxZombies)
            {
                spawnTimer += Time.deltaTime;
                if (spawnTimer >= spawnInterval)
                {
                    SpawnZombie();
                    spawnTimer = 0f;
                }
            }
        }
    }

    void SpawnZombie()
    {
        // Generate a random position offscreen within the spawn radius
        Vector2 spawnPosition = (Vector2)Camera.main.transform.position + Random.insideUnitCircle.normalized * spawnRadius;

        // Instantiate the zombie and increment the counter
        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        currentZombieCount++;
    }

    public void OnZombieDestroyed()
    {
        // Decrement the counter when a zombie is destroyed
        currentZombieCount--;
    }
}
