using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerControls controls;

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float walkingSpeed = 1.5f;
    [SerializeField]
    private float sprintSpeed = 7f;

    private Vector3 velocity;

    private float gravity = -9.81f;

    private Vector2 move;

    private Vector2 sprint;

    private float jumpHeight = 2.4f;

    private CharacterController controller;

    public Transform ground;

    public float distanceToGround = 0.4f;

    public LayerMask groundMask;

    private bool isGrounded;

    private void Awake()
    {
        
        controls = new PlayerControls();
        controller = GetComponent<CharacterController>();   

    }

    private void Update()
    {
        Grav();
        PlayerMovement();
        Jump();
        
    }

    private void Grav()
    {
        isGrounded = Physics.CheckSphere(ground.position, distanceToGround, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }

    private void PlayerMovement()
    {
        move = controls.Movement.GroundMovement.ReadValue<Vector2>();


        Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);
        controller.Move(movement * moveSpeed * Time.deltaTime);



        sprint = controls.Movement.Sprint.ReadValue<Vector2>();

        sprint = move * sprintSpeed;

    }

    
   



    
    
    private void Jump()
    {
        if (controls.Movement.Jump.triggered)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable() 
    {
        controls.Disable();
    }




}
