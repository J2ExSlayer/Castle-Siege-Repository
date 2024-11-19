using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorInteraction : MonoBehaviour
{
    public string doorColor;  // Color of the door (e.g., "Red", "Blue", "Yellow")

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if it's the player
        {
            PlayerKeyInventory playerInventory = other.GetComponent<PlayerKeyInventory>();
            if (playerInventory != null && playerInventory.HasKey(doorColor))  // Check if the player has the right key
            {
                
                Destroy(gameObject);  // Destroy the door if the player has the correct key
                Debug.Log(doorColor + " door opened!");
            }
            else
            {
                Debug.Log("You need the " + doorColor + " key to open this door.");
            }
        }
    }
}
