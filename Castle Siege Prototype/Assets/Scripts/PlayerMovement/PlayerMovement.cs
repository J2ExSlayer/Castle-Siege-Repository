using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerMovement : MonoBehaviour
{

    CharacterController controller;
    public Transform cam;


    public Transform groundCheck;
    public Transform rotationCheck;

    public LayerMask groundMask;

    public LayerMask wallMask;

    public LayerMask climbMask;
    public LayerMask zClimbPosMask;
    public LayerMask zClimbNegMask;
    public LayerMask xClimbPosMask;
    public LayerMask xClimbNegMask;

    

    [SerializeField]
    private Quaternion currentRotation;


    public Collider coll;

    Vector3 move;
    //Vector3 input;
    [SerializeField]
    Vector3 yVelocity;
    Vector3 maxYVelocity;
    Vector3 direction;
    Vector3 wallDirection;
    Vector3 forwardDirection;
    Vector3 upDirection;
    Vector3 zDirection;
    Vector3 xDirection;
    Vector3 wallNormal;
    Vector3 lastWallNormal;
    Vector3 testForwardDirection;

    private Vector3 horizontalMove;
    private Vector3 verticalMove;
    


    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    RaycastHit wallHit;
    RaycastHit climbHit;
    RaycastHit zPosClimbHit;
    RaycastHit zNegClimbHit;
    RaycastHit xPosClimbHit;
    RaycastHit xNegClimbHit;
    RaycastHit frontWallHit;

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

    bool zPosIsWallClimbing;
    bool zPosCanWallClimb;
    bool zPosHasWallClimbed;

    bool zNegIsWallClimbing;
    bool zNegCanWallClimb;
    bool zNegHasWallClimbed;

    bool xPosIsWallClimbing;
    bool xPosCanWallClimb;
    bool xPosHasWallClimbed;

    bool xNegIsWallClimbing;
    bool xNegCanWallClimb;
    bool xNegHasWallClimbed;

    bool canPosZWallClimb;
    bool canNegZWallClimb;
    bool canPosXWallClimb;
    bool canNegXWallClimb;


    bool facingPosX;
    bool facingPosZ;
    bool facingNegX;
    bool facingNegZ;

    bool onFrontWall;
    [SerializeField]
    bool isTestWallClimbing;

    bool isWallJumping;

    [SerializeField]
    int jumpCharges;

    float speed;
    float gravity;
    float climbTimer;
    float wallClimbTimer;
    float var;
    float num1;
    float num2;
    float num3;
    float num4;

    float wallJumpTimer;

    public float runSpeed;
    public float sprintSpeed;
    public float airSpeed;
    public float climbSpeed;
    public float wallClimbSpeed;
    public float moveSpeed;

    public float maxClimbTimer;
    public float maxWallClimbTimer;

    public float wallRunSpeedIncrease;
    public float wallRunSpeedDecrease;

    public float normalGravity;
    public float wallRunGravity;
    public float gravityMax;

    public float jumpHeight;

    public float turnSmoothTime = 0.1f;

    public float maxWallJumpTimer;

    float turnSmoothVelocity;


    // variables for wall jumping
    private Vector3 moveVector;
    private Vector3 lastMove;
    private float wallHopSpeed = 8;
    private float jumpForce = 8;
    //private float gravity = 25;
    private float verticalVelocity;


    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        coll = GetComponent<Collider>();
        currentRotation = gameObject.transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    // Update is called once per frame
    void Update()
    {

        MoveVector();

        //WallClimbingInput();
        //HandleInput();
        CheckWallRun();
        //CheckClimbing();
        //CheckWallClimbing();
        CheckZPosWallClimbing();
        CheckZNegWallClimbing();
        CheckXPosWallClimbing();
        CheckXNegWallClimbing();

        CheckTestWallClimb();

        if (isGrounded)
        {
            HandleInput();
            GroundedMovement();
            ApplyGravity();

        }
        else if (!isGrounded && !isWallRunning && !isClimbing &&
            !zPosIsWallClimbing && !zNegIsWallClimbing && !xPosIsWallClimbing && !xNegIsWallClimbing
            && !isTestWallClimbing)
        {
            HandleInput();
            AirMovement();
            //StartCoroutine(JumpGroundDelay());
            //jumpCharges = 0;
            ApplyGravity();
        }

        else if (isWallRunning)
        {
            HandleInput();
            WallRunMovement();
            jumpCharges = 1;
            //StartCoroutine(JumpDelay());
            //DecreaseSpeed(WallRuneSpeedDecrease);
            ApplyGravity();
        }
        
        else if (isClimbing)
        {
            HandleInput();
            ClimbMovement();
            climbTimer -= 1f * Time.deltaTime;
            if (climbTimer < 0)
            {
                isClimbing = false;
                hasClimbed = true;
            }
            ApplyGravity();
        }
        

        else if (zPosIsWallClimbing)
        {


            ZPosWallClimbingInput();


            WallClimbMovement();
            jumpCharges = 1;
            wallClimbTimer = 1f * Time.deltaTime;
            if (!controller.isGrounded && zPosClimbHit.normal.y < 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.DrawRay(zPosClimbHit.point, zPosClimbHit.normal, Color.red, 1.25f);
                    //verticalVelocity = jumpForce;
                    Jump();
                    move = zPosClimbHit.normal * wallHopSpeed;

                }

            }
            /*
            if (wallClimbTimer < 0)
            {
                zPosIsWallClimbing = false;
                zPosHasWallClimbed = true;


            }
            */
        }
        else if (zNegIsWallClimbing)
        {

            ZNegWallClimbingInput();
            

            WallClimbMovement();
            jumpCharges = 1;
            wallClimbTimer = 1f * Time.deltaTime;
            if (!controller.isGrounded && zNegClimbHit.normal.y < 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.DrawRay(zNegClimbHit.point, zNegClimbHit.normal, Color.red, 1.25f);
                    //verticalVelocity = jumpForce;
                    Jump();
                    move = zNegClimbHit.normal * wallHopSpeed;

                }

            }

            /*
            if (wallClimbTimer < 0)
            {
                zNegIsWallClimbing = false;
                zNegHasWallClimbed = true;

                
            }

            */
        }
        else if (xPosIsWallClimbing)
        {

            XPosWallClimbingInput();
           

            WallClimbMovement();
            jumpCharges = 1;
            wallClimbTimer = 1f * Time.deltaTime;
            if (!controller.isGrounded && xPosClimbHit.normal.y < 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.DrawRay(xPosClimbHit.point, xPosClimbHit.normal, Color.red, 1.25f);
                    //verticalVelocity = jumpForce;
                    Jump();
                    move = xPosClimbHit.normal * wallHopSpeed;

                }

            }
            /*
            if (wallClimbTimer < 0)
            {
                
                xPosIsWallClimbing = false;
                xPosHasWallClimbed = true;

               
            }
            */

        }
        else if (xNegIsWallClimbing)
        {


            XNegWallClimbingInput();

            WallClimbMovement();
            jumpCharges = 1;
            wallClimbTimer = 1f * Time.deltaTime;
            if (!controller.isGrounded && xNegClimbHit.normal.y < 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.DrawRay(xNegClimbHit.point, xNegClimbHit.normal, Color.red, 1.25f);
                    //verticalVelocity = jumpForce;
                    Jump();
                    move = xNegClimbHit.normal * wallHopSpeed;

                }

            }
            /*
            if (wallClimbTimer < 0)
            {
                
                xNegIsWallClimbing = false;
                xNegHasWallClimbed = true;
            }
            */
        }
        
        else if (isTestWallClimbing)
        {

            TestWallClimbingInput();

            TestWallClimbMovement();
            jumpCharges = 1;
            wallClimbTimer = 1f * Time.deltaTime;

            
                if (!controller.isGrounded && frontWallHit.normal.y < 0.1f)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.DrawRay(frontWallHit.point, frontWallHit.normal, Color.red, 1.25f);
                        //verticalVelocity = jumpForce;
                        Jump();
                        move = frontWallHit.normal * wallHopSpeed;

                    }

                }

            

            /*
            if(wallClimbTimer < 0)
            {
                isTestWallClimbing = false;
                isTestWallClimbing = true;
            }
            */
            /*
            if (Input.GetKeyDown(KeyCode.Space) && jumpCharges > 0)
            {
                isTestWallClimbing = false;
                onFrontWall = false;
                //Jump();
                StartCoroutine(ClimbDelay());
                //jumpCharges = 0;
                /*
                TestExitWallClimb();
                if (isTestWallClimbing == true)
                {
                    isTestWallClimbing = false;
                    ApplyGravity();
                }
                */
            /*
        }
    */


        }
        
                //GroundedMovement();
                CheckGround();
                controller.Move(move * Time.deltaTime);


            }

    Vector2 SquareToCircle(Vector2 input)
    {
        return (input.sqrMagnitude >= 1f) ? input.normalized : input;
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


                Vector3 moveDir = Quaternion.Euler(0f, targetAngle /*+ 90f*/ , 0f) * Vector3.forward;
                // had to add back the 90f for the targetAngle variable
                // at this point specifically because it was affecting the movement


                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            } 
        


        if (Input.GetKeyDown(KeyCode.Space) && jumpCharges > 0)
        {

            Jump();
            StartCoroutine(JumpGroundDelay());
            //jumpCharges = 0;
            /*
            TestExitWallClimb();
            if (isTestWallClimbing == true)
            {
                isTestWallClimbing = false;
                ApplyGravity();
            }
            */

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

    /*
    void WallClimbingInput()
    {


        /*
        horizontalMove = Input.GetAxis("Horizontal") * transform.right;
        verticalMove = Input.GetAxis("Vertical") * new Vector3(transform.forward.x, transform.forward.z, 0f);

        Vector3 wallMove = horizontalMove + verticalMove;

        transform.position += wallMove * speed * Time.deltaTime;
        */

        
    /*
        //input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        //input = transform.TransformDirection(input);
        //input = Vector3.ClampMagnitude(input, 1f);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;



        if (direction.magnitude >= 0.1f)
        {
            num1 = -45f;
            num2 = 45f;
            num3 = 135f;
            num4 = -135f;

            // Set Variables for the directions and don't use if statement

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // + cam.eulerAngles.y;



            if () 
            {
                //float targetAngle = Mathf.Atan2(-direction.x, direction.z) * Mathf.Rad2Deg; // + cam.eulerAngles.y;
                //targetAngle = targetAngle;// - 90f;
                // The rotation wants to be at 0 degrees on the x
                // but forward is on the z not the x axis so I needed to rotate -90 degrees for everything to be accurate

                //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                //transform.rotation = Quaternion.Euler(0f, angle, 0f);


                Vector3 moveDir = Quaternion.Euler(0f, 0f, targetAngle) * Vector3.up;
                // had to add back the 90f for the targetAngle variable
                // at this point specifically because it was affecting the movement


                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            
            //if (currentRotation.y > 135f ) 
            //{
                Vector3 moveDir = Quaternion.Euler(0f, 0f, -targetAngle) * Vector3.up;
                // had to add back the 90f for the targetAngle variable
                // at this point specifically because it was affecting the movement


                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            //}
            
         }
        
        /*
        Vector3 hVelocity = controller.velocity;
        hVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 input = SquareToCircle(new Vector2(h, v));

        RaycastHit hit;
        if (Physics.Raycast(transform.position, // Position
                            transform.forward, //Direction
                            out hit))           //Hit Data
        {
            transform.forward = -hit.normal;
            controller.transform.position = Vector3.Lerp(controller.transform.position,
                                                         hit.point + hit.normal * 0.51f,
                                                         10f * Time.deltaTime);
        }

        hVelocity = transform.TransformDirection(input) * climbSpeed;

        */
    /*
    }
    */

    void ZPosWallClimbingInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
      
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; 

             Vector3 moveDir = Quaternion.Euler(0f, 0f, -targetAngle) * Vector3.up;
       
             controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
    void ZNegWallClimbingInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            Vector3 moveDir = Quaternion.Euler(0f, 0f, targetAngle) * Vector3.up;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
    void XPosWallClimbingInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            Vector3 moveDir = Quaternion.Euler(-targetAngle, 0f, 0f) * Vector3.up;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        
    }
    void XNegWallClimbingInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            Vector3 moveDir = Quaternion.Euler(targetAngle, 0f, 0f) * Vector3.up;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
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
        if(isWallJumping)
        {
            move += forwardDirection;
            wallJumpTimer -= 1f * Time.deltaTime;
            if(wallJumpTimer < 0f)
            {
                isWallJumping = false;
            }
        }
    }



    void CheckGround()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if (isGrounded)
        {
            jumpCharges = 1;
            hasWallRun = false;
            hasClimbed = false;
            hasWallClimbed = false;
            zPosHasWallClimbed = false;
            zNegHasWallClimbed = false;
            xPosHasWallClimbed = false;
            xNegHasWallClimbed = false;
            climbTimer = maxClimbTimer;

        }
    }


    void ApplyGravity()
    {
        gravity = isWallRunning ? wallRunGravity : isClimbing ? 0f : 
            zPosIsWallClimbing ? 0f : zNegIsWallClimbing ? 0f : xPosIsWallClimbing ? 0f : xNegIsWallClimbing ? 0f :
            isTestWallClimbing ? 0f : normalGravity;
        yVelocity.y += gravity * Time.deltaTime * 2;
        if(yVelocity.y < gravityMax)
        {
            yVelocity.y = gravityMax;
        }
        controller.Move(yVelocity * Time.deltaTime);
        
    }

    void Jump()
    {
        
        if (!isGrounded && !isWallRunning && !isWallClimbing
            && !zPosIsWallClimbing && !zNegIsWallClimbing && !xPosIsWallClimbing && !xNegIsWallClimbing
            && !isTestWallClimbing)
        {
            //jumpCharges = 0;
            
        }
        else if (isWallRunning)
        {
            ExitWallRun();
            //StartCoroutine(JumpWallDelay());
            //IncreaseSpeed(wallRunSpeedIncrease);
        }
        else if (isWallClimbing)
        {
            TestExitWallClimb();
        }
        else if (zPosIsWallClimbing)
        {
            TestExitWallClimb();
        }
        else if (zNegIsWallClimbing)
        {
            TestExitWallClimb();
        }
        else if (xPosIsWallClimbing)
        {
            TestExitWallClimb();
        }
        else if (xNegIsWallClimbing)
        {
            TestExitWallClimb();
        }
        
        else if (isTestWallClimbing)
        {
            TestExitWallClimb();

        }
        

        //hasWallClimbed = false;
        hasClimbed = false;
        climbTimer = maxClimbTimer;
        hasWallClimbed = false;
        zPosHasWallClimbed = false;
        zNegHasWallClimbed = false;
        xPosHasWallClimbed = false;
        xNegHasWallClimbed = false;
        
        
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

    IEnumerator ClimbDelay()
    {
        yield return new WaitForSeconds(1f);
        onFrontWall = true;
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


    
    void CheckZPosWallClimbing()
    {

        //isWallClimbing = true;
        //jumpCharges = 1;

        zPosCanWallClimb = Physics.Raycast(transform.position, transform.forward, out zPosClimbHit, 0.7f, zClimbPosMask);
        float zPosWallClimbAngle = Vector3.Angle(-zPosClimbHit.normal, transform.forward);
        if (zPosWallClimbAngle < 15 && !zPosHasWallClimbed && zPosCanWallClimb)
        {
            zPosIsWallClimbing = true;
            
            
            
            
        }
        else
        {
            zPosIsWallClimbing = false;
        }

        
        
        

    }
    void CheckZNegWallClimbing()
    {
       // isWallClimbing = true;
        //jumpCharges = 1;

        zNegCanWallClimb = Physics.Raycast(transform.position, transform.forward, out zNegClimbHit, 0.7f, zClimbNegMask);
        float zNegWallClimbAngle = Vector3.Angle(-zNegClimbHit.normal, transform.forward);
        if (zNegWallClimbAngle < 15 && !zNegHasWallClimbed && zNegCanWallClimb)
        {
            zNegIsWallClimbing = true;
            
        }
        else
        {
            zNegIsWallClimbing = false;
        }
        

        

    }
    void CheckXPosWallClimbing()
    {
        /*
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
        */

        //isWallClimbing = true;
        //jumpCharges = 1;

        xPosCanWallClimb = Physics.Raycast(transform.position, transform.forward, out xPosClimbHit, 0.7f, xClimbPosMask);
        float xPosWallClimbAngle = Vector3.Angle(-xPosClimbHit.normal, transform.forward);
        if ( xPosWallClimbAngle < 15 && !xPosHasWallClimbed && xPosCanWallClimb)
        {

            xPosIsWallClimbing = true;
            //TestTestWallClimb();
        }
        else 
        {
            xPosIsWallClimbing = false;
            //TestExitWallClimb();
        }
        

        

    }
    void CheckXNegWallClimbing()
    {
        //isWallClimbing = true;
        //jumpCharges = 1;


        xNegCanWallClimb = Physics.Raycast(transform.position, transform.forward, out xNegClimbHit, 0.7f, xClimbNegMask);
        float xNegWallClimbAngle = Vector3.Angle(-xNegClimbHit.normal, transform.forward);
        if (xNegWallClimbAngle < 15 && !xNegHasWallClimbed && xNegCanWallClimb)
        {
            xNegIsWallClimbing = true;
            
        }
        else
        {
            xNegIsWallClimbing = false;
        }

        

    }

    void WallClimbMovement()
    {

        //What am I trying to do:
        //Take Regular movement using the x and z axis
        //But instead of the z axis use the y
        //to emulate wall climbing
        //while also locking the player forward
        //so they don't slip off when they don't want to
        //only while touching the wall

        //Step 1.
        //make the player move left/right and up/down, unaffected by gravity

        //solution idea: copy paste movement and try switching z with y
        //new problems, make movement axis always reflect direction facing

        //Step 2.
        //make this only while the player is touching the climbing wall

        //Step 3.
        // Lock the player in the direction of the wall, unable to rotate off

        //Step 4.
        //Let the player have a jump off and let go button.

        //input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        //input = transform.TransformDirection(input);
        //input = Vector3.ClampMagnitude(input, 1f);

        /*
        RaycastHit hit;
        if (Physics.Raycast(transform.position,    // Position
                            transform.forward,     // Direction
                            out hit))                         // Hit Data))
        {
            transform.forward = -hit.normal;
            controller.transform.position = Vector3.Lerp(controller.transform.position,
            hit.point + hit.normal * 0.51f,
                10f * Time.fixedDeltaTime);
        }
        */

        isWallClimbing = true;
        jumpCharges = 1;
        
        //IncreaseSpeed(wallRunSpeedIncrease); 
        // I don't know if we want this so I'm leaving it commented for now
        yVelocity = new Vector3(0f, 0f, 0f);


        forwardDirection = Vector3.Cross(wallNormal, Vector3.up);
        
        if (Vector3.Dot(forwardDirection, transform.forward) < 0)
        {
            forwardDirection = -forwardDirection;
        }
        

        speed = wallClimbSpeed;
        if (direction.x != 0)
        {
            move.x += direction.x * speed;

        }
        else
        {
            move.x = 0;
        }


        if (direction.y != 0)
        {
            move.y += direction.y * speed;

        }
        else
        {
            move.y = 0;
        }

        if (direction.z != 0)
        {
            move.z += direction.z * speed;

        }
        else
        {
            move.z = 0;
        }

        //ExitWallClimbing();


        move = Vector3.ClampMagnitude(move, speed);
        
    }

    void ExitWallClimbing()
    {
        isWallClimbing = false;
        StartCoroutine(JumpWallDelay());

        lastWallNormal = wallNormal;
    }

    
    void CheckTestWallClimb()
    {

        /*
        xNegCanWallClimb = Physics.Raycast(transform.position, transform.forward, out xNegClimbHit, 0.7f, xClimbNegMask);
        float xNegWallClimbAngle = Vector3.Angle(-xNegClimbHit.normal, transform.forward);
        if (xNegWallClimbAngle < 15 && !xNegHasWallClimbed && xNegCanWallClimb)
        {
            xNegIsWallClimbing = true;

        }
        else
        {
            xNegIsWallClimbing = false;
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


        */



        onFrontWall = Physics.Raycast(transform.position, transform.forward, out frontWallHit, 0.7f, climbMask); //can climb
        float frontWall = Vector3.Angle(-frontWallHit.normal, transform.forward);

        if ((onFrontWall) && !isTestWallClimbing)
        {
            //isTestWallClimbing = true;
            TestTestWallClimb();
        }
        if((!onFrontWall) && isTestWallClimbing)
        {
            //isTestWallClimbing = false;
            TestExitWallClimb();
        }

    }

    void TestWallClimb()
    {

        /*
         * void WallRun()
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
        */


        isTestWallClimbing = true;
        jumpCharges = 1;

        wallNormal = onFrontWall ? frontWallHit.normal : frontWallHit.normal;
        testForwardDirection = Vector3.Cross(wallNormal, Vector3.up);

        if(Vector3.Dot(testForwardDirection, transform.forward) < 0)
        {
            testForwardDirection = -testForwardDirection;
        }
    }

    void TestTestWallClimb()
    {
        /*
         * void TestWallRun()
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
        */

        wallNormal = onFrontWall ? frontWallHit.normal : frontWallHit.normal;
        if (hasWallClimbed)
        {
            float wallAngle = Vector3.Angle(wallNormal, lastWallNormal);
            if (wallAngle > -1 && wallAngle < 1)
            {
                TestWallClimb();
            }
        }
        else
        {
            TestWallClimb();
            hasWallClimbed = true;
        }
    }

    void TestExitWallClimb()
    {

        /*void ExitWallRun()
    {
        isWallRunning = false;
        StartCoroutine(JumpWallDelay());
        lastWallNormal = wallNormal;
    } */

        isTestWallClimbing = false;
        StartCoroutine(JumpWallDelay());
        lastWallNormal = wallNormal;
        forwardDirection = wallNormal;
        //isWallJumping = true;
        wallJumpTimer = maxWallJumpTimer;

    }

    void TestWallClimbMovement()
    {

        /*void WallRunMovement()
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

    } */

        
        if (direction.z > (forwardDirection.z - 10f) && direction.z < (forwardDirection.z + 10f))
        {
            // move += testForwardDirection;
        }
        else if (direction.z < (forwardDirection.z -10f) && direction.z > (forwardDirection.z + 10f))
        {
            move.x = 0f;
            move.z = 0f;
            TestExitWallClimb();
        }
        move.x += -direction.x * airSpeed;

        move = Vector3.ClampMagnitude(move, speed);
        
    }

    void TestWallClimbingInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            Vector3 moveDir = Quaternion.Euler(-targetAngle, 0f, 0f) * Vector3.up;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Space) && jumpCharges > 0)
        {

            Jump();
            StartCoroutine(JumpGroundDelay());
            //jumpCharges = 0;
            
            

        }
        */
        //HandleInput();

    }

    void MoveVector()
    {
        moveVector = Vector3.zero;
        
        if(controller.isGrounded)
        {
            verticalVelocity = -1;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                //verticalVelocity = jumpForce;
                Jump();
            }
        }
        else
        {
            //verticalVelocity -= gravity * Time.deltaTime;
            moveVector = lastMove;
        }
        moveVector.y = 0;
        moveVector.Normalize();
        moveVector *= wallHopSpeed;
        moveVector.y = verticalVelocity;

        controller.Move(moveVector * Time.deltaTime);
        lastMove = moveVector;

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!controller.isGrounded && hit.normal.y < 0.1f)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
                //verticalVelocity = jumpForce;
                Jump();
                move = hit.normal * wallHopSpeed;
                
            }
            
        }
        
    }

}
