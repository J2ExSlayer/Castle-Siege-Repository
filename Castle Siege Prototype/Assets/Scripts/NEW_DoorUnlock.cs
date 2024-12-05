using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEW_DoorUnlock : MonoBehaviour
{
    public string doorColor; // Store the door's color (blue, red, yellow)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player has the matching key
            if (NEW_PlayerInventory.HasKey(doorColor))
            {
                // Unlock the door and destroy it
                NEW_PlayerInventory.RemoveKey(doorColor); // Remove the key from inventory
                Destroy(gameObject);  // Destroy the door
            }
        }
    }
}
