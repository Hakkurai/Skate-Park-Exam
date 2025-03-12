using UnityEngine;
using TMPro;

public class WinSceneManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Drag UI Text into Inspector

    void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = "Total Score: " + WinTrigger.levelScore; // Display score
        }
        else
        {
            Debug.LogError("Score Text is not assigned in the Inspector!");
        }
    }
}
