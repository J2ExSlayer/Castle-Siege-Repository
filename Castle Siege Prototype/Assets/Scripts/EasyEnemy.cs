//Taylor, Madi
//11/05/2024
//Handles Easy Enemy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EasyEnemy : Enemy
{
    // Start is called before the first frame update

    //How large the circle will be
    public float rad;
    // Start is called before the first frame update
    public override void Move()
    {
        Vector3 tempPos = position;
        //Angle tells us how fast the sphere moves
        float angle = (Time.time * 1 * Mathf.PI);

        //These algorithms control how the sphere moves
        tempPos.x = rad * Mathf.Cos(angle);
        tempPos.z = rad * Mathf.Sin(angle);


        position = tempPos;

        //calls upon the parent class's move function
        base.Move();

    }
}
