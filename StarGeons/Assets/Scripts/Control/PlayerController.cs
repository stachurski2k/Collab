using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control{
public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    [SerializeField] Transform groundCheck;
    [Header("Movement Variables")]
    [SerializeField] float walkSpeed=5f;
    [SerializeField] float sprintMultiplier=1.5f;
    [SerializeField]float crouchMultiplier=.7f;
    [SerializeField] float airControlMultiplier=.6f;
    [SerializeField] float jumpForce=10f;
    [SerializeField] bool useSlope=true;
    Transform cam;
    InputHandler iHandler;
    Rigidbody rBody;
    float currentSpeed;
    float lookBounds=85f;
    float camRot;
    float slopeAngle;
    float disToGround;
    Vector3 velocity;
    Vector3 movementDir;
    RaycastHit groundHit;
    //Movement bools
    bool isGrounded;

    void Start()
    {
        iHandler=GetComponent<InputHandler>();
        cam=Camera.main.transform;
        rBody=GetComponent<Rigidbody>();
        disToGround=groundCheck.position.y+0.1f;
        Cursor.lockState=CursorLockMode.Locked;
        currentSpeed=walkSpeed;
    }

    void Update()
    {
        CalculateLook();
    }

    private void FixedUpdate()
    {
        CalculateMovement();
        GroundCheck();
    }
    
    void CalculateLook(){
        //Rotate Player
        transform.Rotate(Vector3.up*iHandler.GetCameraInput().x*Time.deltaTime);
        //Rotate Camera
        camRot-=iHandler.GetCameraInput().y*Time.deltaTime;
        camRot=Mathf.Clamp(camRot,-lookBounds,lookBounds);
        cam.localRotation=Quaternion.Euler(camRot,0,0);
    }

    void CalculateMovement(){
        movementDir=((transform.forward*iHandler.GetMovement().y)+(transform.right*iHandler.GetMovement().x));

        UpdateSpeed();
        velocity=movementDir*Time.fixedDeltaTime*currentSpeed;

        if(useSlope){
            float normalizedSlopeAngle= (slopeAngle / 90f) * -1f;
            velocity+=normalizedSlopeAngle*velocity;
        }
        rBody.MovePosition(rBody.position+velocity);

        if(isGrounded){
            if(iHandler.IsJumping){
                    rBody.drag = 0f;
                    rBody.velocity = new Vector3(rBody.velocity.x, 0f, rBody.velocity.z);
                    rBody.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
            }
        }
    }

    void UpdateSpeed(){
        currentSpeed=walkSpeed;
        if(isGrounded){
            if(iHandler.IsRunning){
                currentSpeed*=sprintMultiplier;
            }else if(iHandler.IsCrouching){
                currentSpeed*=crouchMultiplier;
            }
        }else{
            currentSpeed*=airControlMultiplier;
        }
    }

    void GroundCheck(){
        if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out groundHit, disToGround,ground)) {
            slopeAngle=Vector3.Angle(groundHit.normal,movementDir.normalized)-90f;
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }
    
}
}