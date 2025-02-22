using UnityEngine;

public class Point : MonoBehaviour
{
    private static int playerScore = 0; // Tracks total points

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            playerScore += 10; // Add 10 points
            Debug.Log("Collected Point! Total Score: " + playerScore);

            Destroy(gameObject); // Remove the prefab after collection
        }
    }
}
