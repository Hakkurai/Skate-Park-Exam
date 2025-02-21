using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;  // Reference to the skateboard (player)
    public Vector3 offset = new Vector3(1, 1.2f, -3);  // Camera offset
    public float smoothSpeed = 0.125f;  // Camera follow smoothness
    public float rotationSpeed = 2f; // Sensitivity of right-click rotation

    private Vector3 currentOffset; // Stores the modified offset
    private float yaw = 0f; // Horizontal camera rotation

    void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
        currentOffset = offset; // Store initial offset
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleCameraRotation();

        // Calculate new desired position
        Vector3 desiredPosition = target.position + currentOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the player
        transform.LookAt(target);
    }

    void HandleCameraRotation()
    {
        if (Input.GetMouseButton(1)) // Right-click is held
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

            // Rotate the offset around the player
            yaw += mouseX;
            Quaternion rotation = Quaternion.Euler(0, yaw, 0);
            currentOffset = rotation * offset;
        }
    }
}
