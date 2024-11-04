using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ClimbingScript : MonoBehaviour
{

    Rigidbody rb;

    CharacterController controller;

    [SerializeField]
    private float speed = 5f;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mVelocity = controller.velocity;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 input = SquareToCircle(new Vector2(h, v));

        if(rb)
        {
          rb.velocity = transform.TransformDirection(input) * speed;
          
        }
        /*
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 input = SquareToCircle(new Vector2(horizontal, vertical));

        if (controller)
        {
            Vector3 direction = transform.TransformDirection(input) * speed;
        }
        */
        
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



        rb.velocity = transform.TransformDirection(input) * speed;
       // mVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
    }

    Vector2 SquareToCircle(Vector3 input)
    {
        return (input.sqrMagnitude >= 1f) ? input.normalized : input;
    }

}
