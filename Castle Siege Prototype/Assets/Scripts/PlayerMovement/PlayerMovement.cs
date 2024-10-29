using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    CharacterController controller;
    public Transform cam;


    public Transform groundCheck;

    public LayerMask groundMask;

    Vector3 move;
    Vector3 input;
    Vector3 Yvelocity;
    Vector3 direction;

    bool isGrounded;
    bool isSprinting;

    int jumpCharges;

    float speed;
    float gravity;

    public float runSpeed;
    public float sprintSpeed;
    public float normalGravity;
    public float airSpeed;
    public float jumpHeight;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }



    // Update is called once per frame
    void Update()
    {

        HandleInput();
        if (isGrounded)
        {
            GroundedMovement();
        }
        else
        {
            AirMovement();
            jumpCharges = 0;
        }


        HandleInput();
        GroundedMovement();
        CheckGround();
        controller.Move(move * Time.deltaTime);
        ApplyGravity();

    }

        void HandleInput()
        {
            //input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            //input = transform.TransformDirection(input);
            //input = Vector3.ClampMagnitude(input, 1f);

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            

        if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                //targetAngle = targetAngle;// - 90f;
                // The rotation wants to be at 0 degrees on the x
                // but forward is on the z not the x axis so I needed to rotate -90 degrees for everything to be accurate

                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);


                Vector3 moveDir = Quaternion.Euler(0f, targetAngle /*+ 90f*/, 0f) * Vector3.forward;
                // had to add back the 90f for the targetAngle variable
                // at this point specifically because it was affecting the movement


                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space) && jumpCharges > 0)
            {

                Jump();


            }

            if(Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
            {
                isSprinting = true;
            }
            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                isSprinting = false;
            }


        }

        void GroundedMovement()
        {
            speed = isSprinting ? sprintSpeed : runSpeed;
            if (direction.x != 0)
            {
                move.x += direction.x * speed;

            }
            else
            {
                move.x = 0 ;
            }

            
            if (direction.z != 0)
            {
                move.z += direction.z * speed;

            }
            else
            {
                move.z = 0;
            }

            move = Vector3.ClampMagnitude(move, speed);

        }

        void AirMovement()
        {
            move.x += direction.x * airSpeed;
            move.z += direction.z * airSpeed;
        }



        void CheckGround()
        {

            isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
            if (isGrounded)
            {
                jumpCharges = 1;

            }
        }


        void ApplyGravity()
        {
            gravity = normalGravity;
            Yvelocity.y += gravity * Time.deltaTime;
            controller.Move(Yvelocity * Time.deltaTime);
        }

        void Jump()
        {

            Yvelocity.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);

        }





    
}
