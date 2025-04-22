using UnityEngine;
using UnityEngine.UI;

public class CampfireInteraction : MonoBehaviour
{
    public GameObject uiPanel; // Reference to the UI panel to show
    public Transform player;  // Reference to the player's transform
    public float interactionDistance = 2f; // Max distance for interaction
    public GameObject campFirePrefab;
    private bool isNearCampfire = false;

    void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); // Hide the UI panel at the start
        }
    }

    void Update()
    {
        // Check distance between player and campfire
        float distance = Vector3.Distance(player.position, transform.position);
        isNearCampfire = distance <= interactionDistance;
        
        Item selectedItem = InventroyManager.instance.GetSelectedItem(false);

        if (isNearCampfire && Input.GetMouseButtonDown(0) && selectedItem.actionType == ActionType.Cookable) // Left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ToggleUI();
            }
        }
         if(isNearCampfire && Input.GetMouseButtonDown(0) && selectedItem.actionType == ActionType.Break)
         {
            Instantiate(campFirePrefab, transform.position, Quaternion.identity);
            DestroyImmediate(gameObject);
        }
    }

    void ToggleUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(!uiPanel.activeSelf); // Toggle UI visibility
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize interaction range in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
