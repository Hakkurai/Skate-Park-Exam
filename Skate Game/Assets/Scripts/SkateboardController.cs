using UnityEngine;

public class SkateboardController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float gravity = 9.81f;
    public float rotationSpeed = 10f;
    public float slopeForce = 10f;
    public float slopeRaycastLength = 2f; // Increased raycast length for better slope detection

    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.slopeLimit = 45f;
        controller.stepOffset = 0.3f;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Convert movement to be relative to the player's forward direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        if (move.magnitude > 1) move.Normalize();

        // Apply movement speed
        moveDirection.x = move.x * moveSpeed;
        moveDirection.z = move.z * moveSpeed;

        // Adjust movement and rotation based on slope
        AlignToSlope();

        // Rotate player towards movement direction smoothly
        if (move.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Jump logic: Allow jumping only when grounded
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
            }
            else
            {
                moveDirection.y = -1f;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the character
        controller.Move(moveDirection * Time.deltaTime);
    }

    void AlignToSlope()
    {
        if (!isGrounded) return;

        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Ensure ray starts above the ground
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, slopeRaycastLength))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            // Check if the player is on a slope but within limits
            if (slopeAngle > 0 && slopeAngle < controller.slopeLimit)
            {
                Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, Time.deltaTime * rotationSpeed);

                // Apply extra force to help the player clear the slope
                if (moveDirection.z > 0) // Moving forward
                {
                    moveDirection += hit.normal * (slopeForce * Time.deltaTime);
                }
            }
        }
    }
}
