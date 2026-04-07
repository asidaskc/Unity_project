using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform player;      // Reference to the player's transform
    public float followSpeed = 2f; // Speed at which the camera follows the player
    public Vector3 offset;         // Offset position of the camera relative to the player

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the desired position
            Vector3 desiredPosition = player.position + offset;

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}