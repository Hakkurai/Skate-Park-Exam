using UnityEngine;
using System.Collections;

public class SkateboardController : MonoBehaviour
{
    public float speed = 12f;
    public float turnSpeed = 60f;
    public float jumpForce = 8f;
    public float extraGravity = 20f;
    public float rampForceMultiplier = 1.5f;
    public float speedBoostAmount = 100f; // Boost amount
    public float speedBoostDuration = 5f; // Boost lasts 5 seconds
    public Transform groundCheck;
    public float groundCheckDistance = 0.6f;
    public LayerMask groundLayer;
    public LayerMask slopeLayer;
    public float slopeAdjustmentSpeed = 12f;
    public float maxTiltAngle = 30f;
    public float antiTiltForce = 10f;
    public float stabilizationForce = 500f;
    public float slopeLandingForce = 5f; // Adjusts landing force on slopes

    private Rigidbody rb;
    private bool isGrounded;
    private bool isOnSlope;
    private bool isSpeedBoostActive = false;
    private Vector3 normalVector = Vector3.up;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.angularDamping = 15f;
        rb.linearDamping = 0.2f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        GroundCheck();
        MovePlayer();
        ApplyExtraGravity();
        ApplyStabilizationForces();
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
            if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0 || ((1 << hit.collider.gameObject.layer) & slopeLayer) != 0)
            {
                isGrounded = true;
                normalVector = hit.normal;

                if (((1 << hit.collider.gameObject.layer) & slopeLayer) != 0)
                {
                    isOnSlope = true;
                    AdjustToSlope(normalVector);
                }
            }
        }
    }

    void AdjustToSlope(Vector3 groundNormal)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, groundNormal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * slopeAdjustmentSpeed);
    }

    void MovePlayer()
    {
        float currentSpeed = isSpeedBoostActive && Input.GetKey(KeyCode.LeftShift) ? speed + speedBoostAmount : speed;
        float moveInput = Input.GetAxis("Vertical") * currentSpeed;
        float turnInput = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;

        Vector3 moveDirection;

        if (isOnSlope)
        {
            moveDirection = Vector3.ProjectOnPlane(transform.forward, normalVector).normalized * rampForceMultiplier;
        }
        else
        {
            moveDirection = transform.forward;
        }

        rb.AddForce(moveDirection * moveInput, ForceMode.Acceleration);
        transform.Rotate(0, turnInput, 0);
    }

    void ApplyExtraGravity()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
    }

    void ApplyStabilizationForces()
    {
        if (isGrounded)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
            rb.AddForce(-transform.right * localVelocity.x * stabilizationForce * Time.fixedDeltaTime, ForceMode.Force);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    // ✅ Fix for getting stuck when landing on slopes
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Slope"))
        {
            isGrounded = true;
            rb.linearVelocity = Vector3.zero;

            if (isOnSlope)
            {
                rb.AddForce(-normalVector * slopeLandingForce, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBoost"))
        {
            StartCoroutine(ActivateSpeedBoost());
            Destroy(other.gameObject);
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

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Slope"))
        {
            isGrounded = false;
            isOnSlope = false;
        }
    }
}
