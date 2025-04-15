using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject playerHotbar; // Reference to the player's hotbar

    private GameObject currentOpenUI; // Keeps track of the currently open UI

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI(inventoryUI);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI(optionsUI);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleUI(craftingUI);
        }
    }

    private void ToggleUI(GameObject uiElement)
    {
        if (uiElement != null)
        {
            if (currentOpenUI == uiElement)
            {
                // If closing optionsUI, re-enable the player's hotbar
                if (uiElement == optionsUI)
                {
                    playerHotbar.SetActive(true);
                }

                uiElement.SetActive(false);
                currentOpenUI = null;
            }
            else
            {
                // Close all UI if optionsUI is being activated
                if (uiElement == optionsUI)
                {
                    CloseAllUI();
                }

                // Close the currently active UI
                if (currentOpenUI != null)
                {
                    currentOpenUI.SetActive(false);
                }

                // Activate the new UI element
                uiElement.SetActive(true);
                currentOpenUI = uiElement;
            }
        }
    }

    private void CloseAllUI()
    {
        inventoryUI.SetActive(false);
        craftingUI.SetActive(false);
        playerHotbar.SetActive(false); // Deactivate the player's hotbar
        currentOpenUI = null;
    }
}
