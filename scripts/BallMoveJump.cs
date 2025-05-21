using Oculus.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BallMoveJump : MonoBehaviour
{
    public InputActionReference moveInput;  // z. B. primary2DAxis (Linker Stick)
    public InputActionReference jumpInput;  // z. B. Button South
    public Camera sphereCam;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private Vector2 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDirection = moveInput.action.ReadValue<Vector2>();
        if (jumpInput.action.triggered && IsGrounded())
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move(moveDirection);
    }

    public void Move(Vector2 direction)
    {
        // see https://learn.unity.com/course/roll-a-ball/tutorial/moving-the-player?version=2022.3
        Vector3 movement = new Vector3(direction.x, 0f, direction.y);
        // Vector3 targetPosition = rb.position + move * moveSpeed * Time.fixedDeltaTime;

        //this Vector3 will ensure that the sphere will head always in the direction where the camera is looking
        Vector3 actualDirection = sphereCam.transform.TransformDirection(movement);
        rb.AddForce(actualDirection * moveSpeed);

        // rb.AddForce(movement * moveSpeed); // OLD 
        // rb.MovePosition(targetPosition);
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning("GroundCheck transform is not assigned.");
            return true;
        }
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
