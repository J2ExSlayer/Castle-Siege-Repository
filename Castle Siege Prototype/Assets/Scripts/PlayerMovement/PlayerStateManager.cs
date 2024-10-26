using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{

    public CharacterController controller;

    public PlayerInput input;


    public Vector3 moveVector;

    public Vector2 inputVector;

    public float playerSpeed;

    public float playerRotateSpeed;

    private Vector3 _gravityVector;









    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        playerSpeed = 10f;
        playerRotateSpeed = 180;

        _gravityVector = new Vector3(0, -9.81f, 0);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        //RotateTowardsVector();
        ApplyGravity();
        
    }


    public void ApplyGravity()
    {
        controller.Move(_gravityVector * Time.deltaTime);
    }




    public void Move()
    {
        controller.Move(playerSpeed * moveVector * Time.deltaTime);
    }

    public void RotateTowardsVector()
    {
        var xzDirection = new Vector3(moveVector.x, 0, moveVector.z);
        if (xzDirection.magnitude == 0) return;

        var rotation = Quaternion.LookRotation(xzDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, playerRotateSpeed);
    }



    private void OnGroundMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        moveVector.x = inputVector.x;
        moveVector.z = inputVector.y;


    }


}
