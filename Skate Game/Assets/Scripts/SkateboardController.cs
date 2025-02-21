using UnityEngine;
using System.Collections;

public class SkateboardController : MonoBehaviour //scripting
{
    public float speed = 10f;
    public float turnSpeed = 50f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;
    public LayerMask slopeLayer; // Separate layer for slopes
    public float slopeAdjustmentSpeed = 10f;
    public float speedBoostAmount = 100f; // Extra speed added
    public float speedBoostDuration = 5f; // Duration of boost

    private Rigidbody rb;
    private bool isGrounded;
    private bool isOnSlope;
    private bool isSpeedBoostActive = false;
    private Vector3 normalVector = Vector3.up;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        GroundCheck();
        MovePlayer();
    }

    void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void GroundCheck()
    {
        isGrounded = false;
        isOnSlope = false;

        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundCheckDistance))
        {
            if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
            {
                isGrounded = true;
                normalVector = hit.normal;
            }
            else if (((1 << hit.collider.gameObject.layer) & slopeLayer) != 0)
            {
                isOnSlope = true;
                normalVector = hit.normal;
                AdjustToSlope(normalVector);
            }
        }
    }

    void AdjustToSlope(Vector3 groundNormal)
    {
        if (isOnSlope)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, groundNormal), groundNormal);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * slopeAdjustmentSpeed);
        }
    }

    void MovePlayer()
    {
        float currentSpeed = speed;

        // If speed boost is active and Shift is held, apply the boost
        if (isSpeedBoostActive && Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed += speedBoostAmount;
        }

        float moveInput = Input.GetAxis("Vertical") * currentSpeed;
        float turnInput = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;

        rb.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        transform.Rotate(0, turnInput, 0);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Slope"))
        {
            isGrounded = true;
            rb.linearVelocity = Vector3.zero;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Slope"))
        {
            isGrounded = false;
            isOnSlope = false;
        }
    }

    // ?? Speed Boost Activation
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBoost"))
        {
            StartCoroutine(ActivateSpeedBoost());
            Destroy(other.gameObject); // Remove the speed boost object from the scene
        }
    }

    private IEnumerator ActivateSpeedBoost()
    {
        isSpeedBoostActive = true;
        Debug.Log("Speed Boost Activated!");

        yield return new WaitForSeconds(speedBoostDuration);

        isSpeedBoostActive = false;
        Debug.Log("Speed Boost Ended.");
    }
}
