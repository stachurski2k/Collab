using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] float mouseSensitivity=0.5f;
    Vector2 lookInput;
    Vector2 movement;
    bool Jump,Crouch,Run;
    public Vector2 GetCameraInput(){
        return lookInput*mouseSensitivity;
    }
    public Vector2 GetMovement(){
        //optionaly some blocking etc.
        return movement;
    }
    public bool IsRunning{
        get{return Run;}
    }
    public bool IsCrouching{
        get{return Crouch;}
    }
    public bool IsJumping{
        get{return Jump;}
    }
    #region MethodsCalledByPlayerController
    public void OnLook(InputValue value){
        lookInput=value.Get<Vector2>();
    }
    public void OnMove(InputValue value){
        movement=value.Get<Vector2>();
    }
    public void OnJump(InputValue value){
        Jump=value.isPressed;
    }
    public void OnCrouch(InputValue value){
        Crouch=value.isPressed;
    }
    public void OnRun(InputValue value){
        Run=value.isPressed;
    }
    #endregion
}
