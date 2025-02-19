using UnityEngine;

public class SkateboardController : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 50f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        GroundCheck();
        MoveKart();
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
        if (isGrounded)
        {
            Debug.Log("Ground detected!");
        }
    }

    void MoveKart()
    {
        float moveInput = Input.GetAxis("Vertical") * speed;
        float turnInput = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;

        rb.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        transform.Rotate(0, turnInput, 0);
    }
}