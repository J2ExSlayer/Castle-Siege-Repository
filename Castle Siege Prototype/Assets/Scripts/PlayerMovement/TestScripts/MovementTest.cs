using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{

    private Vector3 horizontalMove;
    private Vector3 verticalMove;

    public float moveSpeed;

    private void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal") * transform.right;
        verticalMove = Input.GetAxis("Vertical") * new Vector3(transform.forward.x, transform.forward.z, 0f);

        Vector3 direction = horizontalMove + verticalMove;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }


}
