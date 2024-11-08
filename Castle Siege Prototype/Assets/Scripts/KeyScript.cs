using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{

    public Component doorHingehere;
    public GameObject KeyGone;
    
    void OnTriggerStay()
    {
        if (Input.GetKey(KeyCode.E))
            doorHingehere.GetComponent<BoxCollider>().enabled = true;
        if (Input.GetKey(KeyCode.E))
            KeyGone.SetActive(false);
    }

    
}
