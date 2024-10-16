using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVariables : MonoBehaviour
{

    public CharacterController controller;

    public PlayerInput input;


    public Vector3 moveVector;

    public Vector2 inputVector;

    public float playerSpeed;

    public float playerRotateSpeed;

    private Vector3 _gravityVector;



}
