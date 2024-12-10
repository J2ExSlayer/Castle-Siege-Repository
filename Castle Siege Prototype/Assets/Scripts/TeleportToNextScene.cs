using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TeleportToNextScene : MonoBehaviour
{
    // Reference to the UI script (you can attach this in the Inspector)
    public string scenename;

    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with this GameObject is the player
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(scenename);
        }
    }
}
