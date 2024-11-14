using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyInventory : MonoBehaviour
{
    private List<string> collectedKeys = new List<string>();  // List of collected keys

    // Call this method when the player collects a key
    public void CollectKey(string keyColor)
    {
        if (!collectedKeys.Contains(keyColor))
        {
            collectedKeys.Add(keyColor);  // Add key to inventory if not already collected
            Debug.Log("Collected " + keyColor + " key.");
        }
    }

    // Check if the player has a specific key
    public bool HasKey(string keyColor)
    {
        return collectedKeys.Contains(keyColor);
    }
}
