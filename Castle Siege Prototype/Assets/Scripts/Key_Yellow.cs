using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Yellow : MonoBehaviour
{
    // Flag to check if the player has collected the key
    private bool hasKey = false;

    // Reference to the door GameObject (for later destruction)
    public GameObject doorYellow;

    // Reference to the key GameObject (for optional destruction after collection)
    public GameObject keyYellow;

    // When the player enters a collider (trigger)
    void OnTriggerEnter(Collider other)
    {
        // If the player enters the key's trigger
        if (other.CompareTag("KeyYellow") && !hasKey)
        {
            CollectKey(other.gameObject); // Collect the key
        }

        // If the player enters the door's trigger and they have the key
        if (other.CompareTag("DoorYellow") && hasKey)
        {
            DestroyDoor(); // Destroy the door
        }
    }

    // Collect the key
    void CollectKey(GameObject keyObject)
    {
        hasKey = true;  // Player now has the key
        Debug.Log("Key Collected!");

        // Optionally destroy the key object from the scene
        Destroy(keyObject);

        // You can also disable the key object if you don't want to destroy it:
        // keyObject.SetActive(false);
    }

    // Destroy the door when the player has the key
    void DestroyDoor()
    {
        if (doorYellow != null)
        {
            Debug.Log("Door Destroyed!");
            Destroy(doorYellow);  // Destroy the door object
        }
    }
}
