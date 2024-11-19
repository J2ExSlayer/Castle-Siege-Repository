using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentDestroy : MonoBehaviour
{
    // This will be called when the player steps on the object
    void OnTriggerEnter(Collider other)
    {
        // Check if the object that triggered the event is the player
        if (other.CompareTag("Player"))
        {
            // Destroy this object when the player steps on it
            Destroy(gameObject);
        }
    }
}
