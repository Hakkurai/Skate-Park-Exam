using UnityEngine;
using TMPro;

public class Consumable : MonoBehaviour
{
    public int points = 10; // The points added per collection
    private static int score = 0; // Keeps track of score
    public TextMeshProUGUI scoreText; // Reference to the UI text

    private void Start()
    {
        // Find the TextMeshPro UI in the scene if not assigned
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        }

        UpdateScoreUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure player has a tag "Player"
        {
            score += points; // Add 10 points
            UpdateScoreUI();
            Destroy(gameObject); // Destroy the consumable
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
