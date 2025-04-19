using UnityEngine;
using UnityEngine.Tilemaps;

public class OnHill : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public Tilemap terrainTilemap; // Reference to the terrain tilemap
    public float hillZoomSize = 10f; // Zoom size when on hill tiles
    public float normalZoomSize = 5f; // Normal zoom size
    public float zoomSpeed = 2f; // Speed of zoom transition

    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has the "Player" tag
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        Vector3Int playerPosition = terrainTilemap.WorldToCell(playerTransform.position);
        TileBase tile = terrainTilemap.GetTile(playerPosition);

        if (tile != null && IsHillTile(tile))
        {
            // Smoothly zoom to hillZoomSize
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, hillZoomSize, Time.deltaTime * zoomSpeed);
        }
        else
        {
            // Smoothly zoom to normalZoomSize
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, normalZoomSize, Time.deltaTime * zoomSpeed);
        }
    }

    bool IsHillTile(TileBase tile)
    {
        // Add your logic to identify hill tiles. For example:
        // return tile.name == "HillTile";
        return true; // Replace with your actual check
    }
}
