using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    CharacterController controller;
    public Transform cam;


    public Transform groundCheck;

    public LayerMask groundMask;

    public LayerMask wallMask;

    public LayerMask climbMask;

    Vector3 move;
    //Vector3 input;
    Vector3 yVelocity;
    Vector3 direction;
    Vector3 forwardDirection;
    Vector3 wallNormal;
    Vector3 lastWallNormal;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    RaycastHit wallHit;
    RaycastHit climbHit;

    bool isGrounded;
    bool isSprinting;

    bool isWallRunning;
    bool onLeftWall;
    bool onRightWall;
    bool hasWallRun = false;
    bool hasClimbWall = false;

    bool isClimbing;
    bool canClimb;
    bool hasClimbed;

    bool isWallClimbing;
    bool canWallClimb;
    bool hasWallClimbed;

    [SerializeField]
    int jumpCharges;

    float speed;
    float gravity;
    float climbTimer;
    float wallClimbTimer;

    public float runSpeed;
    public float sprintSpeed;
    public float airSpeed;
    public float climbSpeed;
    public float wallClimbSpeed;

    public float maxClimbTimer;
    public float maxWallClimbTimer;

    public float wallRunSpeedIncrease;
    public float wallRunSpeedDecrease;

    public float normalGravity;
    public float wallRunGravity;

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
        CheckWallRun();
        CheckClimbing();
        CheckWallClimbing();
        if(isGrounded)
        {
            GroundedMovement();

        }
        else if(!isGrounded && !isWallRunning && !isClimbing && !isWallClimbing)
        {
            AirMovement();
            //StartCoroutine(JumpGroundDelay());
            //jumpCharges = 0;
        }

        else if(isWallRunning)
        {
            WallRunMovement();
            jumpCharges = 1;
            //StartCoroutine(JumpDelay());
            //DecreaseSpeed(WallRuneSpeedDecrease);
        }
        else if(isClimbing)
        {
            ClimbMovement();
            climbTimer -= 1f * Time.deltaTime;
            if(climbTimer < 0)
            {
                isClimbing = false;
                hasClimbed = true;
            }
        }
        else if (isWallClimbing)
        {
            WallClimbMovement();
            wallClimbTimer -= 1f * Time.deltaTime;
            if (wallClimbTimer < 0)
            {
                isWallClimbing = false;
                hasWallClimbed = true;
            }
        }

        //GroundedMovement();
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
            StartCoroutine(JumpGroundDelay());
            //jumpCharges = 0;


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
            hasWallRun = false;
            hasClimbed = false;
            climbTimer = maxClimbTimer;

        }
    }


    void ApplyGravity()
    {
        gravity = isWallRunning ? wallRunGravity : isClimbing ? 0f : isWallClimbing ? 0f : normalGravity;
        yVelocity.y += gravity * Time.deltaTime;
        controller.Move(yVelocity * Time.deltaTime);
    }

    void Jump()
    {
        
        if (!isGrounded && !isWallRunning)
        {
            //jumpCharges = 0;
            
        }
        else if (isWallRunning)
        {
            ExitWallRun();
            //StartCoroutine(JumpWallDelay());
            //IncreaseSpeed(wallRunSpeedIncrease);
        }
        
        hasClimbed = false;
        climbTimer = maxClimbTimer;
        hasWallClimbed = false;
        wallClimbTimer = maxWallClimbTimer;

        yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);
        //jumpCharges = 0;

    }

    IEnumerator JumpGroundDelay()
    {
        yield return new WaitForSeconds(0.1f);
        jumpCharges = 0;
    }

    IEnumerator JumpWallDelay()
    {
        yield return new WaitForSeconds(0.5f);
        jumpCharges = 0;
    }

    void CheckWallRun()
    {

        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.7f, wallMask);
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, 0.7f, wallMask);

        if ((onRightWall || onLeftWall) && !isWallRunning)
        {
            TestWallRun();
        }
        if ((!onRightWall && !onLeftWall) && isWallRunning)
        {
            ExitWallRun();
        }



    }

    void WallRun()
    {
        isWallRunning = true;
        jumpCharges = 1;
        //IncreaseSpeed(wallRunSpeedIncrease); 
        // I don't know if we want this so I'm leaving it commented for now
        yVelocity = new Vector3(0f, 0f, 0f);

        
        forwardDirection = Vector3.Cross(wallNormal, Vector3.up);

        if(Vector3.Dot(forwardDirection, transform.forward) < 0)
        {
            forwardDirection = -forwardDirection;
        }

    }

    void ExitWallRun()
    {
        isWallRunning = false;
        StartCoroutine(JumpWallDelay());
        lastWallNormal = wallNormal;
    }

    void WallRunMovement()
    {
        if (direction.z > (forwardDirection.z -10f) && direction.z < (forwardDirection.z + 10f))
        {
            move += forwardDirection;
        }
        else if (direction.z <  (forwardDirection.z - 10f) && direction.z > (forwardDirection.z +10f))
        {
            move.x = 0f;
            move.z = 0f;
            ExitWallRun();
        }
        move.x += direction.x * airSpeed;

        move = Vector3.ClampMagnitude(move, speed);

    }

    void TestWallRun()
    {
        wallNormal = onLeftWall ? leftWallHit.normal : rightWallHit.normal;
        if(hasWallRun)
        {
            float wallAngle = Vector3.Angle(wallNormal, lastWallNormal);
            if (wallAngle > 15)
            {
                WallRun();
            }
        }
        else
        {
            WallRun();
            hasWallRun = true;
        }

    }
    

    void CheckClimbing()
    {
        canClimb = Physics.Raycast(transform.position, transform.forward, out wallHit, 0.7f, wallMask);
        float wallAngle = Vector3.Angle(-wallHit.normal, transform.forward);
        if (wallAngle < 15 && !hasClimbed && canClimb)
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

    }

    void ClimbMovement()
    {
        forwardDirection = Vector3.up;
        move.x += direction.x * airSpeed;
        move.z += direction.z * airSpeed;

        yVelocity += forwardDirection;
        speed = climbSpeed;

        move = Vector3.ClampMagnitude(move, speed);
        yVelocity = Vector3.ClampMagnitude(yVelocity, speed);
    }

    void CheckWallClimbing()
    {
        canWallClimb = Physics.Raycast(transform.position, transform.forward, out climbHit, 0.7f, climbMask);
        float wallClimbAngle = Vector3.Angle(-climbHit.normal, transform.forward);
        if (wallClimbAngle < 15 && !hasWallClimbed && canWallClimb)
        {
            isWallClimbing = true;
        }
        else
        {
            isWallClimbing = false;
        }

    }

    void WallClimbMovement()
    {
        forwardDirection = Vector3.up;
        move.x += direction.x * airSpeed;
        move.y += direction.y * airSpeed;

        yVelocity += forwardDirection;
        speed = wallClimbSpeed;

        move = Vector3.ClampMagnitude(move, speed);
        yVelocity = Vector3.ClampMagnitude(yVelocity, speed);
    }


}
