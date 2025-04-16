using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite mainWallSprite;
    public Sprite topRightCornerSprite;
    public Sprite topLeftCornerSprite;
    public Sprite bottomRightCornerSprite;
    public Sprite bottomLeftCornerSprite;
    public Sprite bottomWallSprite;
    public Sprite leftWallSprite;
    public Sprite rightWallSprite;

    private SpriteRenderer spriteRenderer;

    // Neighbor detection flags
    public bool hasTopWall;
    public bool hasBottomWall;
    public bool hasLeftWall;
    public bool hasRightWall;

    private BuildManager buildManager; // Reference to BuildManager for grid tracking

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildManager = FindAnyObjectByType<BuildManager>(); // Find BuildManager in the scene

        DetectNeighbors(); // Dynamically detect neighbors based on grid
        UpdateWallSprite(); // Assign the correct sprite
    }
    void Update()
    {
        DetectNeighbors();
        UpdateWallSprite();
    }

    void DetectNeighbors()
    {
        Vector2Int currentTile = WorldToTilePosition(transform.position);

        // Calculate positions of neighboring tiles
        Vector2Int topTile = new Vector2Int(currentTile.x, currentTile.y + 1);
        Vector2Int bottomTile = new Vector2Int(currentTile.x, currentTile.y - 1);
        Vector2Int leftTile = new Vector2Int(currentTile.x - 1, currentTile.y);
        Vector2Int rightTile = new Vector2Int(currentTile.x + 1, currentTile.y);

        // Check if neighboring tiles are occupied
        hasTopWall = buildManager.IsTileOccupied(topTile);
        hasBottomWall = buildManager.IsTileOccupied(bottomTile);
        hasLeftWall = buildManager.IsTileOccupied(leftTile);
        hasRightWall = buildManager.IsTileOccupied(rightTile);

        // Debug logs for validation
        Debug.Log($"Neighbors for tile {currentTile} - Top: {hasTopWall}, Bottom: {hasBottomWall}, Left: {hasLeftWall}, Right: {hasRightWall}");
    }

void UpdateWallSprite()
{
    // Logic to decide wall type based on neighbors
    if (!hasTopWall && !hasBottomWall && hasLeftWall && !hasRightWall)
    {
        spriteRenderer.sprite = mainWallSprite; 
    }
    else if (!hasTopWall && !hasBottomWall && hasRightWall && !hasLeftWall)
    {
        spriteRenderer.sprite = mainWallSprite; 
    }
    else if (!hasTopWall && !hasBottomWall && hasRightWall && hasLeftWall)
    {
        spriteRenderer.sprite = mainWallSprite; 
    }
    else if (!hasBottomWall && hasTopWall && !hasLeftWall && hasRightWall)
    {
        spriteRenderer.sprite = bottomLeftCornerSprite;
    }
    else if (!hasBottomWall && hasTopWall && !hasRightWall && hasLeftWall)
    {
        spriteRenderer.sprite = bottomRightCornerSprite; 
    }
    else if (hasBottomWall && !hasTopWall && !hasRightWall && hasLeftWall)
    {
        spriteRenderer.sprite = topLeftCornerSprite; 
    }
    else if (hasBottomWall && !hasTopWall && hasRightWall && !hasLeftWall)
    {
        spriteRenderer.sprite = topRightCornerSprite; 
    }
    else if (hasTopWall && hasBottomWall && !hasLeftWall && !hasRightWall)
    {
        spriteRenderer.sprite = leftWallSprite; 
    }
    else if (hasTopWall && !hasBottomWall && !hasLeftWall && !hasRightWall)
    {
        spriteRenderer.sprite = leftWallSprite; 
    }

}

    Vector2Int WorldToTilePosition(Vector3 position)
    {
        // Convert world position to tile position (reuse BuildManager logic)
        int x = Mathf.FloorToInt(position.x / buildManager.tileSize);
        int y = Mathf.FloorToInt(position.y / buildManager.tileSize);
        return new Vector2Int(x, y);
    }
}
