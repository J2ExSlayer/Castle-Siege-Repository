using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeDoor : MonoBehaviour
{
    public Animation hingeHere;

    void OnTriggerStay()
    {
        if (Input.GetKey(KeyCode.E))
            hingeHere.Play();
    }
}
