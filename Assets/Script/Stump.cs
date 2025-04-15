using UnityEngine;

public class StumpScript : MonoBehaviour
{
    public GameObject itemPrefab; // Item to drop upon breaking
    public float hitsToBreak = 3f;
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.1f;
    public float interactionRange = 5f; // Distance within which the player can interact

    private float currentHits = 0f;
    private Vector3 originalPosition;

    private Transform playerTransform; // Reference to the player's transform

    private void Start()
    {
        // Save the original position of the stump for shake effect
        originalPosition = transform.position;

        // Locate the player by their tag
        playerTransform = GameObject.FindWithTag("Player").transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player object with tag 'Player' not found! Ensure the player has the 'Player' tag.");
        }
    }

    private void OnMouseDown()
    {
        // Check if the player is nearby
        if (IsPlayerNearby())
        {
            // Validate if the selected tool is a shovel
            Item selectedItem = InventroyManager.instance.GetSelectedItem(false);
            if (selectedItem != null && selectedItem.type == ItemType.Shovel && selectedItem.actionType == ActionType.Break)
            {
                StartCoroutine(ShakeStump()); // Add a shake effect
                currentHits++;

                // Destroy the stump and drop the item if the required hits are reached
                if (currentHits >= hitsToBreak)
                {
                    DropItem();
                    Destroy(gameObject); // Remove the stump from the scene
                }
            }
            else
            {
                Debug.Log("You need a shovel to break the stump!");
            }
        }
        else
        {
            Debug.Log("You are too far away to interact with the stump!");
        }
    }

    private bool IsPlayerNearby()
    {
        // Calculate the distance between the player and the stump
        return Vector3.Distance(playerTransform.position, transform.position) <= interactionRange;
    }

    private void DropItem()
    {
        // Instantiate the item prefab at the stump's position
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
    }

    private System.Collections.IEnumerator ShakeStump()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Apply random lateral movement for the shake effect
            transform.position = originalPosition + (Vector3.right * Random.Range(-shakeIntensity, shakeIntensity));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the stump's position to its original state
        transform.position = originalPosition;
    }
}
