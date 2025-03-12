using UnityEngine;
using TMPro;

public class Consumable : MonoBehaviour
{
    public int points = 10; // Points per consumable
    public TextMeshProUGUI scoreText; // Reference to UI text

    private void Start()
    {
        // Automatically find the UI text if not assigned
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        }
        UpdateScoreUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WinTrigger.levelScore += points; // Update global level score
            UpdateScoreUI();
            Destroy(gameObject);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + WinTrigger.levelScore; // Use static score
        }
    }
}
