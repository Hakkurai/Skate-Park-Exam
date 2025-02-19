using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;  // Reference to the skateboard (player)
    public Vector3 offset = new Vector3(0, 5, -10);  // Adjust the offset as needed
    public float smoothSpeed = 0.125f;  // Smoothing factor for the camera movement

    void Start()
    {
        // If no target is assigned, find the GameObject tagged as "Player"
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

        // Calculate the desired position based on the target's position and offset
        Vector3 desiredPosition = target.position + offset;
        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}
