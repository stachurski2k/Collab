﻿using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float moveSpeed = 0.5f;
    float scrollSpeed = 10f;

    void Update()
    {
        InputKeys();
    }

    void InputKeys()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            transform.position += moveSpeed * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            transform.position += scrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
        }
    }
}