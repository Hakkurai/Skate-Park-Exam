using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class Point : MonoBehaviour
{
    private static int playerScore = 0; // Tracks total points
    private static TextMeshProUGUI scoreText; // Reference to TMP UI text

    private void Start()
    {
        // Find the TMP text in the scene if not already assigned
        if (scoreText == null)
        {
            GameObject scoreObject = GameObject.Find("ScoreText"); // Ensure this matches your TMP text object's name
            if (scoreObject != null)
            {
                scoreText = scoreObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("ScoreText TMP object not found in scene!");
            }
        }

        // Update the score text initially
        UpdateScoreUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerScore += 10; // Add 10 points
            Debug.Log("Collected Point! Total Score: " + playerScore);

            UpdateScoreUI(); // Update the UI text

            Destroy(gameObject); // Remove the prefab after collection
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + playerScore;
        }
    }
}
