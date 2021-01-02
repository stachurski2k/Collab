using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control{
public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    CharacterController characterController;
    //TO DO
    //InputHandler inputHandler;
     float maxForwardSpeed=4f;
     float turnSpeed=10f;
     float jumpSpeed=10f;
     float crouchSpeedFactor=0.5f;
    float aimSpeedFraction=0.7f;
     float runSpeedFactor=1.5f;
    float desiredForwardSpeed;
    float verticalSpeed=0f;
    float currentVelocity;
    bool isGrounded=true;
    bool isCrouching=false;
    bool readyToJump=false;
    bool isRunning=false;
    Vector3 additiveForce;
    float currentSpeed;
    Quaternion targetRotation;
    //These are the values i think will be needed
    void Start()
    {
        //inputHandler=GetComponent<InputHandler>();
        characterController=GetComponent<CharacterController>();
    }

    void Update()
    {
        //To do : main Loop
    }
    //To do : Use it as collider
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
    }
}
}