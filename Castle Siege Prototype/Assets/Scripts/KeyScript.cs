using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject doorCollider;
    void Start()
    {
        doorCollider.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            doorCollider.SetActive(true);
            Destroy(gameObject);

        }
    }
}
