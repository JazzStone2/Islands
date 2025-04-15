using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player to follow
    public float smoothSpeed = 0.125f; // The smoothing speed
    public Vector3 offset; // Offset from the player's position

    void FixedUpdate()
    {
        if (player == null) return; // Ensure the player is set

        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.position + offset;

        // Smoothly transition to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
