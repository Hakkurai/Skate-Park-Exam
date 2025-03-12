using UnityEngine;

public class SkateboardController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
    public float jumpForce = 10f;
    public float doubleJumpMultiplier = 1.5f;
    public float gravityMultiplier = 2.5f;
    public float fallMultiplier = 2f;
    public float rampStickForce = 20f; // Force to keep skateboard on ramp
    public LayerMask groundLayer;
    public LayerMask rampLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isOnRamp;
    private int jumpCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        CheckGrounded();
        HandleJump();
        ApplyExtraGravity();
        HandleReset();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || jumpCount < 2)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

                float jumpPower = (jumpCount == 1) ? jumpForce * doubleJumpMultiplier + 2f : jumpForce;
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                
                jumpCount++;
                isGrounded = false;
                isOnRamp = false;
                Debug.Log(jumpCount == 1 ? "Player Jumped" : "Player Double Jumped with Extra Speed!");
            }
        }
    }
    void HandleReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            rb.angularVelocity = Vector3.zero; // Stops any spinning
            Debug.Log("Player Reset Rotation");
        }
    }

    void ApplyExtraGravity()
    {
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.down * gravityMultiplier * fallMultiplier, ForceMode.Acceleration);
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        float rayLength = 1f;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayLength, groundLayer | rampLayer))
        {
            if (!isGrounded)
            {
                jumpCount = 0;
                Debug.Log("Jump count reset");
            }
            isGrounded = true;
            isOnRamp = (rampLayer & (1 << hit.collider.gameObject.layer)) != 0;
        }
        else
        {
            isGrounded = false;
            isOnRamp = false;
        }

        if (isOnRamp)
        {
            Vector3 rampNormal = hit.normal;
            rb.AddForce(-rampNormal * rampStickForce, ForceMode.Acceleration);
        }

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, isGrounded ? Color.green : Color.red);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            rb.linearDamping = 0.1f;
            isOnRamp = true;
        }
        else if ((groundLayer & (1 << collision.gameObject.layer)) != 0)
        {
            isGrounded = true;
            jumpCount = 0;
            isOnRamp = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            rb.linearDamping = 1f;
            isOnRamp = false;
        }
    }
}
