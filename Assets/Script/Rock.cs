using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject itemDropPrefab; // Prefab for the item dropped when the rock is broken
    public int health = 3; // The number of hits the rock can take before breaking
    public float shakeIntensity = 0.1f; // Intensity of the shaking effect
    public float shakeDuration = 0.2f; // Duration of the shaking effect
    public float interactionRange = 2f; // Distance within which the player can interact with the rock

    private Vector3 originalPosition;
    private bool isShaking = false;
    private GameObject player;

    void Start()
    {
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player"); // Ensure your player has the "Player" tag
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= interactionRange && Input.GetMouseButtonDown(0)) // Left mouse button to interact
        {
            // Assuming the player has a tool equipped and can check the tool type
            ItemType toolType = GetPlayerToolType(); // Custom method to get the player's equipped tool
            if (toolType == ItemType.Tool || toolType == ItemType.Weapon)
            {
                HitWithTool(toolType);
            }
        }
    }

    private ItemType GetPlayerToolType()
    {
        // Replace this with your actual logic to retrieve the player's equipped tool
        // This is just a placeholder
        return ItemType.Tool;
    }

    public void HitWithTool(ItemType toolType)
    {
                    Item selectedItem = InventroyManager.instance.GetSelectedItem(false);

            if (selectedItem != null && selectedItem.type == ItemType.pickaxe && selectedItem.actionType == ActionType.Break)        {
            ShakeRock();
            health--;

            if (health <= 0)
            {
                BreakRock();
            }
        }
        else
        {
            Debug.Log("This tool can't break rocks!");
        }
    }

    private void ShakeRock()
    {
        if (isShaking)
            return;

        isShaking = true;
        StartCoroutine(ShakeCoroutine());
    }

    private System.Collections.IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * shakeIntensity);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
    }

    private void BreakRock()
    {
        Debug.Log("Rock broken!");

        if (itemDropPrefab != null)
        {
            Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
