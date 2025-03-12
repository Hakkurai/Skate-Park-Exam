using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelSelectScene = "LevelSelect"; // Name of the level select scene

    public void PlayGame()
    {
        SceneManager.LoadScene(levelSelectScene); // Load the level select screen
        Debug.Log("Play button clicked! Loading Level Select...");
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked! Exiting game...");
        Application.Quit(); // Quits the game (only works in build)
    }
}
