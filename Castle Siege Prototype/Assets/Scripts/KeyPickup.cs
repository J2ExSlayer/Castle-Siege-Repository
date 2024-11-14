using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public string keyColor;  // Color of the key (e.g., "Red", "Blue", "Yellow")

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure the player has the "Player" tag
        {
            PlayerKeyInventory inventory = other.GetComponent<PlayerKeyInventory>();
            if (inventory != null)
            {
                inventory.CollectKey(keyColor);  // Collect the key
                Destroy(gameObject);  // Destroy the key object once collected
                Debug.Log(keyColor + " key collected!");
            }
        }
    }
}
