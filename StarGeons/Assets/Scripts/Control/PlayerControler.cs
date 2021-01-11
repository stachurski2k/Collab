using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState{IDLE,WALKING,CROUCHING,RUNNING,SLIDING,}
[RequireComponent(typeof(InputHandler))]
public class PlayerControler : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    [SerializeField] Transform groundCheck;
    [Header("Movement Variables")]
    [SerializeField] float walkSpeed=5f;
    [SerializeField] float sprintMultiplier=1.5f;
    [SerializeField]float crouchMultiplier=.7f;
    [SerializeField] float airControlMultiplier=.6f;
    [SerializeField] float jumpForce=10f;
    [SerializeField] float gravity=20f;
    [Header("Camera Variables")]
    [SerializeField] Vector2 smothing=new Vector2(3,3);
    CharacterController controller;  
    InputHandler input;
    Transform cam;
    bool isGrounded=true;
    void Start()
    {
        input=GetComponent<InputHandler>();
        cam=Camera.main.transform;
    }

    void Update()
    {
        UpdateCam();
    }
    void UpdateCam(){
        
    }
}
