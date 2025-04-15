using UnityEngine;

public class TorchPickup : MonoBehaviour
{
    public GameObject spawnPrefab; // Assign the prefab to spawn in the Inspector
    public float pickupRadius = 2f; // Define the radius within which the player can pick up the torch

    private Transform playerTransform; // Reference to the player's transform

    private void Start()
    {
        // Locate the player in the scene (assumes the player has the "Player" tag)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Perform a raycast from the mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Check if the raycast hits this torch and if the player is close enough
            if (hit.collider != null && hit.collider.gameObject == gameObject &&
                playerTransform != null && Vector3.Distance(playerTransform.position, transform.position) <= pickupRadius)
            {
                PickUp();
            }
            else
            {
            }
        }
    }

    private void PickUp()
    {
        if (spawnPrefab != null)
        {
            // Spawn the GameObject at the torch's position and rotation
            Instantiate(spawnPrefab, transform.position, transform.rotation);

            // Destroy the torch GameObject after spawning
            Destroy(gameObject);
        }
        else
        {
            
        }
    }
}
