using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player; // Automatically assigned
    public Vector3 offset = new Vector3(0, 3, -5); // Adjustable offset
    public float smoothSpeed = 5f;

    void Start()
    {
        FindPlayer();
    }

    void LateUpdate()
    {
        if (player == null)
        {
            FindPlayer();
            if (player == null) return; // If still null, exit
        }

        FollowPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FollowPlayer()
    {
        // Target position based on player's rotation and offset
        Vector3 targetPosition = player.position + player.rotation * offset;

        // Smoothly interpolate position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Ensure the camera always looks at the player
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
