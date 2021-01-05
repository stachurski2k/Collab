using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control{
public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadious;
    [Header("Movement Variables")]
    [SerializeField] float walkSpeed=5f;
    [SerializeField] float sprintMultiplier=1.5f;
    [SerializeField]float crouchMultiplier=.7f;
    [SerializeField] float jumpForce=10f;
    [SerializeField] float gravity=-9.81f;
    Camera cam;
    InputHandler iHandler;
    float lookBounds=85f;
    float camRot;
    float verticalSpeed;
    float slopeAngle;
    Vector3 velocity;
    RaycastHit groundHit;
    //Movement bools
    bool isCrouching,isSprinting;
    bool isGrounded;
    bool jumpRequest;
    void Start()
    {
        iHandler=GetComponent<InputHandler>();
        cam=Camera.main;
    }

    void Update()
    {
        CalculateLook();
        CalculateMovement();
        SetPosition();
        GroundCheck();
    }
    void CalculateLook(){
        transform.Rotate(Vector3.up*iHandler.GetCameraInput().x);
        camRot+=iHandler.GetCameraInput().y;
        camRot=Mathf.Clamp(camRot,-lookBounds,lookBounds);
        cam.transform.localEulerAngles=Vector3.left*camRot;

    }
    void CalculateMovement(){
        velocity=((transform.forward*iHandler.GetMovement().y)+(transform.right*iHandler.GetMovement().x))
        *Time.deltaTime*walkSpeed;

        float normalizedSlopeAngle= (slopeAngle / 90f) * -1f;
        velocity+=slopeAngle*velocity;

        if(isGrounded){
            verticalSpeed=0;
            if(iHandler.JumpInput){
                verticalSpeed=jumpForce;
            }
        }else{
            verticalSpeed+=gravity*Time.deltaTime;
        }
        velocity+=transform.up*verticalSpeed*Time.deltaTime;
    }
    void SetPosition(){
        transform.position+=velocity;
    }
    void GroundCheck(){
        if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out groundHit, groundCheckRadious,ground)) {
            slopeAngle=Vector3.Angle(groundHit.normal,transform.forward)-90f;
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }
    
}
}