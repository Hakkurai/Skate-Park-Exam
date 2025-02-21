using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;  // Reference to the skateboard (player)
    public Vector3 offset = new Vector3(1, 1.2f, -3);  // Camera offset
    public float smoothSpeed = 0.125f;  // Camera follow smoothness
    public float rotationSmoothSpeed = 5f; // Camera rotation smoothness

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
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate new camera position based on player position
        Vector3 desiredPosition = target.position + target.rotation * offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Smoothly rotate the camera to match the player's direction
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);
    }
}
