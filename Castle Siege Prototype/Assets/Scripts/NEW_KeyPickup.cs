using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEW_KeyPickup : MonoBehaviour
{
    public string keyColor; // Store the key's color (blue, red, yellow)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // if the player doesn't already have the key
            if (!NEW_PlayerInventory.HasKey(keyColor))
            {
                NEW_PlayerInventory.AddKey(keyColor);  // Add the key to inventory
                Destroy(gameObject);  // Destroy the key after pickup
            }
        }
    }
}
