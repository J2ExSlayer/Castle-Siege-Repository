using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEW_PlayerInventory : MonoBehaviour
{
    private static List<string> keys = new List<string>(); // A list to hold the player's keys

    // Check if player already has the key
    public static bool HasKey(string keyColor)
    {
        return keys.Contains(keyColor);
    }

    // Add a key to the inventory
    public static void AddKey(string keyColor)
    {
        if (!keys.Contains(keyColor))
        {
            keys.Add(keyColor);
        }
    }

    // Remove a key from the inventory (used when opening the door)
    public static void RemoveKey(string keyColor)
    {
        if (keys.Contains(keyColor))
        {
            keys.Remove(keyColor);
        }
    }
}
