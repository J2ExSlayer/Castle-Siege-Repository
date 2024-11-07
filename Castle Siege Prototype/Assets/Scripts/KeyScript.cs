using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public Component doorcolliderhere;
    public GameObject KeyGone;

    void OnTriggerStay()
    {
        if (Input.GetKeyUp(KeyCode.E)) ;

    }

}
