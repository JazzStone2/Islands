using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public GameObject stumpPrefab;
    public GameObject woodPrefab;
    public GameObject applePrefab;

    public float hitsToBreak = 3f;
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.1f;

    public float appleCooldownTime = 900f;
    public float interactionRange = 5f;

    private float currentHits = 0f;
    private Vector3 originalPosition;
    private bool canDropApple = true;

    private Transform playerTransform;

    private float appleDropHeight = 0.5f; // Fixed drop height for apples
    private float groundLevel;

    private void Start()
    {
        originalPosition = transform.position;
        playerTransform = GameObject.FindWithTag("Player").transform;

        groundLevel = transform.position.y - 1f; // Set ground level relative to tree
    }

    private void OnMouseDown()
    {
        if (IsPlayerNearby())
        {
            Item selectedItem = InventroyManager.instance.GetSelectedItem(false);

            if (selectedItem != null && selectedItem.type == ItemType.Axe && selectedItem.actionType == ActionType.Break)
            {
                StartCoroutine(ShakeTree());
                currentHits++;

                if (currentHits >= hitsToBreak)
                {
                    Instantiate(stumpPrefab, transform.position, transform.rotation);
                    int dropCount = Random.Range(1, 4);

                    for (int i = 0; i < dropCount; i++)
                    {
                        Instantiate(woodPrefab, transform.position, Quaternion.identity);
                    }

                    Destroy(gameObject);
                }
            }
            else if (canDropApple)
            {
                StartCoroutine(ShakeTree());
                DropApple();
                StartAppleCooldown();
            }
        }
    }

    private bool IsPlayerNearby()
    {
        return Vector3.Distance(playerTransform.position, transform.position) <= interactionRange;
    }

    private void DropApple()
    {
        // Set apple spawn position
        Vector3 applePosition = new Vector3(transform.position.x, transform.position.y + appleDropHeight, transform.position.z);


        GameObject apple = Instantiate(applePrefab, applePosition, Quaternion.identity);

        Rigidbody2D rb = apple.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = apple.AddComponent<Rigidbody2D>();
        }

        rb.freezeRotation = true;
        rb.gravityScale = 1f;

        // Ensure the apple lands directly on the ground
        StartCoroutine(SnapToGroundLevel(apple));
    }

    private System.Collections.IEnumerator SnapToGroundLevel(GameObject apple)
    {
        Rigidbody2D rb = apple.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            yield break;
        }

        while (true)
        {
            yield return null;

            // Check if the apple is at or below the ground level
            if (apple.transform.position.y <= groundLevel)
            {
                // Snap apple to the ground and remove Rigidbody2D for stability
                apple.transform.position = new Vector3(apple.transform.position.x, groundLevel, apple.transform.position.z);
                Destroy(rb); // Remove Rigidbody2D to keep the apple stationary
                yield break;
            }
        }
    }

    private System.Collections.IEnumerator ShakeTree()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            transform.position = originalPosition + (Vector3.right * Random.Range(-shakeIntensity, shakeIntensity));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }

    private void StartAppleCooldown()
    {
        canDropApple = false;
        Invoke(nameof(ResetAppleCooldown), appleCooldownTime);
    }

    private void ResetAppleCooldown()
    {
        canDropApple = true;
    }
}
