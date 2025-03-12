using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene("MainGame"); // Change "MainGame" to your actual scene name
        Debug.Log("Loading Level 1...");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level_2"); // Change "Level_2" to your actual scene name
        Debug.Log("Loading Level 2...");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level_3"); // Change "Level_3" to your actual scene name
        Debug.Log("Loading Level 3...");
    }
}
