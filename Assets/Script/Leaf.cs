using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    public GameObject leafPrefab; // Assign your leaf prefab in the Inspector
    public Transform treePosition; // Central position for spawning leaves
    public Vector2 spawnAreaSize = new Vector2(5f, 2f); // Size of the spawn area (width, height)
    public float spawnInterval = 2.0f; // Time between leaf spawns

    private void Start()
    {
        // Start spawning leaves at regular intervals
        InvokeRepeating(nameof(SpawnLeaf), spawnInterval, spawnInterval);
    }

    private void SpawnLeaf()
    {
        // Generate a random position within the spawn area
        Vector3 spawnPosition = new Vector3(
            treePosition.position.x + Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2), 
            treePosition.position.y + Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2), 
            treePosition.position.z
        );

        // Instantiate the leaf prefab at the random spawn position with no rotation
        GameObject leaf = Instantiate(leafPrefab, spawnPosition, Quaternion.identity);

        // Add a custom leaf-falling script to control its behavior
        leaf.AddComponent<LeafMovement>();
    }
}

public class LeafMovement : MonoBehaviour
{
    public float fallSpeed = 2.0f; // Falling speed
    public float swayAmplitude = 0.5f; // Swaying horizontal amplitude
    public float swayFrequency = 1.0f; // Swaying speed
    public float rotationSpeed = 50.0f; // Rotation speed
    public float lifespan = 5.0f; // Time before the leaf despawns
    public Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f); // Minimum scale before destroying

    private Vector3 startPosition;
    private float time;
    private float lifeTimer;
    private Vector3 originalScale;

    private void Start()
    {
        startPosition = transform.position; // Record the starting position
        time = Random.Range(0f, 2f * Mathf.PI); // Randomize the sway phase for each leaf
        lifeTimer = lifespan; // Initialize the lifespan timer
        originalScale = transform.localScale; // Store the original scale
    }

    private void Update()
    {
        // Simulate falling by moving down
        transform.position = new Vector3(
            startPosition.x + Mathf.Sin(time * swayFrequency) * swayAmplitude, // Horizontal sway
            transform.position.y - fallSpeed * Time.deltaTime, // Falling down
            startPosition.z
        );

        // Add rotation for a natural effect
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Gradually shrink the leaf over its lifespan
        float scaleProgress = lifeTimer / lifespan; // Calculate how far along the lifespan is
        transform.localScale = Vector3.Lerp(minScale, originalScale, scaleProgress);

        // Reduce the life timer and destroy the leaf when time is up
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }

        // Increment time for smooth movement
        time += Time.deltaTime;
    }
}
