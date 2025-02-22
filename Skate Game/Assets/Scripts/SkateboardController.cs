using System.Collections;
using UnityEngine;

public class SkateboardController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
    public float jumpForce = 10f;
    public float gravityMultiplier = 2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        CheckGrounded();
        HandleJump();
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Vertical") * moveSpeed;
        float turn = Input.GetAxis("Horizontal") * turnSpeed;

        Vector3 forwardMove = transform.forward * move * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMove);

        transform.Rotate(Vector3.up, turn * Time.deltaTime);
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset Y velocity to prevent weak jumps
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Set to false immediately after jumping to prevent double jumps
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        float rayLength = 1.1f; // Shortened Raycast to avoid misdetection
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Offset to prevent clipping

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength, groundLayer))
        {
            isGrounded = true;
            rb.linearDamping = 1f; // Add slight drag when grounded
        }
        else
        {
            isGrounded = false;
            rb.linearDamping = 0f; // No drag in air
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, isGrounded ? Color.green : Color.red);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            rb.linearDamping = 0.5f; // Reduce friction on ramps for smooth sliding
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            rb.linearDamping = 1f; // Reset drag when leaving the ramp
        }
    }
}
