using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    // Reference to the PlayerSpawnPoint in Level 2
    public Transform playerSpawnPoint;

    void Start()
    {
        // This positions the player in loadedposition 
        if (SceneManager.GetActiveScene().name == "Level1")  
        {
            TeleportPlayerToSpawn();
        }
    }

    void TeleportPlayerToSpawn()
    {
        //spawn point is assigned and teleport the player to that position
        if (playerSpawnPoint != null)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            playerTransform.position = playerSpawnPoint.position;
            playerTransform.rotation = playerSpawnPoint.rotation;  
        }
    }
}
