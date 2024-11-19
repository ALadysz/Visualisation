using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController3D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializedField] private float moveSpeed = 5f;
    [SerializedField] private float jumpHeight = 1.5f;
    [SerializedField] private float gravity = -9.81f;

    [Header("Camera Settings")]
    [SerializedField] private Transform cameraTransform;
    [SerializedField] private float mouseSensitivity = 100f;
    [SerializedField] private float maxLookAngle = 80f;

    [Header("Ground Check")]
    [SerializedField] private Transform groundCheck;
    [SerializedField] private float groundCheckRadius = 0.3f;
    [SerializedField] private LayerMask groundLayer;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private float cameraPitch = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        //lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
        HandleCameraControl();
    }

    private void HandleMovement()
    {
        //ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //reset downward velocity
        }

        //get input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //calculate movement direction relative to camera
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        //handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCameraControl()
    {
        //get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        //rotate camera vertically
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }
}
