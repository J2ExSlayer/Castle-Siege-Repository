using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

   
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z ) * Mathf.Rad2Deg + cam.eulerAngles.y;
            targetAngle = targetAngle - 90f;
            // The rotation wants to be at 0 degrees on the x
            // but forward is on the z not the x axis so I needed to rotate -90 degrees for everything to be accurate

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f); 
            
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle + 90f, 0f) * Vector3.forward;
            // had to add back the 90f for the targetAngle variable
            // at this point specifically because it was affecting the movement
            

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
