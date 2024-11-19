using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotatorDetector : MonoBehaviour
{
    // Rotation speed of the enemy
    public float rotationSpeed = 50f;

    // The detection range for the enemy (field of view or proximity cone)
    public float detectionRange = 5f;

    // The field of view angle (cone width in degrees)
    public float fieldOfViewAngle = 60f;

    // Reference to the player's transform
    public Transform player;

    // Whether the player is detected
    private bool playerDetected = false;

    // Optional: For debugging purposes, show the field of view in the Scene view
    private void OnDrawGizmos()
    {
        // Draw the detection range (a sphere representing the area the enemy can detect in)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualize the field of view (cone of detection)
        Gizmos.color = new Color(1, 1, 0, 0.5f); // A semi-transparent yellow color
        Vector3 forward = transform.forward; // The forward direction of the enemy

        // Calculate the cone's width and draw lines to represent the cone
        float angleStep = fieldOfViewAngle / 2f;

        for (float angle = -angleStep; angle <= angleStep; angle += 5f)
        {
            // Calculate the direction of each line in the cone
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
            Gizmos.DrawLine(transform.position, transform.position + direction * detectionRange);
        }

        // Optionally: Draw a central line to show the direct forward direction
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + forward * detectionRange);
    }

    private void Update()
    {
        // Rotate the enemy around its own axis
        RotateEnemy();

        // Check if the player is within the detection range and field of view
        DetectPlayer();
    }

    private void RotateEnemy()
    {
        // Rotate the enemy around its Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void DetectPlayer()
    {
        if (player == null) return;

        // Calculate the direction from the enemy to the player
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Check if the player is within the detection range
        if (distanceToPlayer <= detectionRange)
        {
            // Calculate the angle between the enemy's forward direction and the direction to the player
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // If the player is within the field of view angle, and the distance is within range
            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                // The player is within the detection cone
                playerDetected = true;
                Debug.Log("Player Detected!");
                // Optionally, trigger a function to handle player detection, such as an alert or alert animation.
            }
            else
            {
                playerDetected = false;
            }
        }
        else
        {
            playerDetected = false;
        }
    }

    // Optionally, you can use OnTriggerEnter to detect player collision with the proximity cone (if needed).
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Detection Range");
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Left Detection Range");
            playerDetected = false;
        }
    }
}
