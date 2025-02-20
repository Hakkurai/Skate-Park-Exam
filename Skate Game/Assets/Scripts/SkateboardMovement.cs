using UnityEngine;

public class SkateboardController : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 50f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;
    public LayerMask slopeLayer; // Separate layer for slopes
    public float slopeAdjustmentSpeed = 10f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isOnSlope;
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
            Debug.Log("Raycast Hit: " + hit.collider.name + " | Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

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
                Debug.Log("Board detected slope: Adjusting angle.");
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
        float moveInput = Input.GetAxis("Vertical") * speed;
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
}
