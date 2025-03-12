using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public string winSceneName = "Win_1"; // Win scene name
    public static int levelScore = 0; // Stores the total level score

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Only allow player to trigger
        {
            Debug.Log("Player reached the goal! Loading Win Scene with score: " + levelScore);
            SceneManager.LoadScene(winSceneName);
        }
    }
}
