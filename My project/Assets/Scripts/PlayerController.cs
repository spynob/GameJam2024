
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.5f;

    //camer for first person perspective
    public Camera childCamera;
    private Transform cameraTransform;

    //looking around
    public float lookSensitivity = 200.0f;
    public float lookSmoothing = 2.0f;
    private Vector2 lookDirection;
    private float verticalMaxAngle = 75.0f;
    private float verticalRotation;

    //translation
    private Vector3 moveAmount;
    private Vector3 smoothMoveVelocity;

    //jumping
    public LayerMask groundedMask;
    private float jumpForce = 200f;
    private bool grounded;


    private void Start()
    {
        //Turn off cursor visibility, confine it to the game window
        Cursor.lockState = CursorLockMode.Locked;

        cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        ControlLookAround();

        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 targetMoveAmount = moveDirection * speed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        ControlJump();

        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            //show the cursor, unlock it from the game window
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void FixedUpdate()
    {
        //need to convert from global space to player's coordinate axes. 
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    private void ControlLookAround()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * lookSensitivity);
        verticalRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalMaxAngle, verticalMaxAngle);
        cameraTransform.localEulerAngles = Vector3.left * verticalRotation;
    }

    private void ControlJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody>().AddForce(transform.up * jumpForce);
        }

        grounded = false;
        Ray ray = new Ray(transform.position, -cameraTransform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, GetComponent<CapsuleCollider>().height / 2 + 0.1f, groundedMask))
        {
            grounded = true;
        }
    }
}