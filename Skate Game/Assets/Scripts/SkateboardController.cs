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

    private int jumpCount = 0; // Tracks jumps (0 = grounded, 1 = first jump, 2 = double jump)

    private bool isFirstJump = true; // Tracks if this is the player's first jump ever

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded) // Normal ground jump
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset Y velocity

                // Apply normal jump for the first-ever jump, stronger after that
                float jumpPower = isFirstJump ? jumpForce : jumpForce * 1.2f;
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

                isFirstJump = false; // Mark that the player has now jumped at least once
                jumpCount = 1; // First jump performed

                Debug.Log(isFirstJump ? "Player's Initial Jump" : "Player Jumped with Slight Boost");
            }
            else if (jumpCount == 1) // Double jump
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jumpForce * 1.5f, ForceMode.Impulse); // 1.5x stronger double jump
                jumpCount++;

                Debug.Log("Player Double Jumped with Extra Force!");
            }
        }
    }




    void CheckGrounded()
    {
        RaycastHit hit;
        float rayLength = 1f; // Shortened Raycast to avoid misdetection
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Offset to prevent clipping

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength, groundLayer))
        {
            if (!isGrounded) // Only reset when touching the ground for the first time
            {
                jumpCount = 0; // Reset jump count when landing
                Debug.Log("Jump count reset");
            }

            isGrounded = true;
            rb.linearDamping = 1f; // Add slight drag when grounded
            Debug.Log("Player is Grounded");
        }
        else
        {
            isGrounded = false;
            rb.linearDamping = 0f; // No drag in air
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
            Debug.Log("Player is Airborne");
        }

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, isGrounded ? Color.green : Color.red);
    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            rb.linearDamping = 0.1f; // Reduce friction on ramps for smooth sliding
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
