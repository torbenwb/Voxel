using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Vector3 aimRotation = Vector3.zero;
    public float moveSpeed = 10f;
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        RotateAim();
        Move();
    }

    void RotateAim(){
        float xInput = Input.GetAxisRaw("Mouse X");
        float yInput = Input.GetAxisRaw("Mouse Y");

        aimRotation.x = Mathf.Clamp(aimRotation.x + yInput * -180f * Time.deltaTime, -89f, 89f);
        aimRotation.y += xInput * 180f * Time.deltaTime;
        Camera.main.transform.rotation = Quaternion.Euler(aimRotation);
    }

    void Move(){
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = Camera.main.transform.forward * moveY + Camera.main.transform.right * moveX;
        moveDirection.y += Input.GetKey(KeyCode.Space) ? 1f : 0f;
        moveDirection.y -= Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;
        Camera.main.transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }
}
