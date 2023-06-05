using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float CameraRatio = 0.8f;

    void Update()
    {
        // Get the position between the player and the mouse cursor
        Vector3 targetPosition = GetTargetPosition();

        transform.position = targetPosition + new Vector3(0, 1, -5);
    }

    Vector3 GetTargetPosition()
    {
        // Get the position of the mouse cursor in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Assuming a 2D environment

        // Calculate the position between the player and the mouse cursor
        Vector3 targetPosition = CameraRatio * player.transform.position + (1-CameraRatio) * mousePosition;

        return targetPosition;
    }
}
