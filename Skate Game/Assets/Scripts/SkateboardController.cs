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
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f, groundLayer))
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
