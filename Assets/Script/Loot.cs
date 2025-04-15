using System.Collections;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr; // Assign in the Inspector
    [SerializeField] private new BoxCollider2D collider; // Assign in the Inspector
    [SerializeField] private float moveSpeed = 5f; // Default move speed
    [SerializeField] private float floatAmplitude = 0.5f; // Amplitude of floating
    [SerializeField] private float floatFrequency = 1f; // Frequency of floating
    [SerializeField] private bool enableFloating = false; // Flag to enable/disable floating
    [SerializeField] private Item item;
    [SerializeField] private GameObject shadowPrefab; // Assign in the Inspector

    private GameObject shadowInstance; // Store the shadow instance
    private Vector3 startPosition;
    private Vector3 initialShadowScale; // Store the initial scale of the shadow

    private void Start()
    {
        startPosition = transform.position; // Store the initial position

        if (item != null)
        {
            sr.sprite = item.image; // Use the item's sprite to set the SpriteRenderer's sprite
        }

        if (shadowPrefab != null)
        {
            // Instantiate the shadow prefab slightly lower beneath the item
            shadowInstance = Instantiate(shadowPrefab, transform.position + new Vector3(0f, -0.3f, 0f), Quaternion.identity);
            shadowInstance.transform.parent = transform; // Parent the shadow to the item
            initialShadowScale = shadowInstance.transform.localScale; // Record the shadow's initial scale
        }
    }

    private void Update()
    {
        if (enableFloating)
        {
            // Apply floating motion in the vertical direction
            transform.position = startPosition + new Vector3(0f, Mathf.Sin(Time.time * floatFrequency) * floatAmplitude, 0f);

            if (shadowInstance != null)
            {
                // Adjust shadow position to stay farther below the item
                shadowInstance.transform.position = transform.position + new Vector3(0f, -0.3f, 0f);

                // Dynamically scale the shadow based on the item's vertical movement
                float scaleAdjustment = 1f - (Mathf.Sin(Time.time * floatFrequency) * 0.1f);
                shadowInstance.transform.localScale = initialShadowScale * scaleAdjustment;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool canAdd = InventroyManager.instance.AddItem(item);
            if (canAdd)
            {
                StartCoroutine(MoveAndCollect(other.transform));
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator MoveAndCollect(Transform target)
    {
        collider.enabled = false; // Disable the collider to avoid re-triggering
        if (shadowInstance != null)
        {
            Destroy(shadowInstance); // Destroy the shadow upon collection
        }
        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }
        Destroy(gameObject); // Destroy this GameObject after collection
    }
}
