//Taylor, Madi
//11/05/2024
//Handles Enemy Parent
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    public float emSpeed;


    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    // Start is called before the first frame update

    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        Vector3 tempPos = position;
        tempPos.y -= emSpeed * Time.deltaTime;
        position = tempPos;
    }



}