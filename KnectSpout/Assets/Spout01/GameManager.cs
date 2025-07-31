using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button pauseButton;
    public Button reloadButton;
    private bool isPaused = false;

    void Start()
    {
        // Add listeners to the buttons
        pauseButton.onClick.AddListener(TogglePause);
        reloadButton.onClick.AddListener(ReloadScene);
    }

    // Toggle the game's pause state
    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Debug.Log("Game Paused");  // Debug log to check when pause is triggered
            Time.timeScale = 0;  // Pause the game
        }
        else
        {
            Debug.Log("Game Resumed");  // Debug log to check when unpause is triggered
            Time.timeScale = 1;  // Resume the game
        }
    }

    // Reload the current scene
    void ReloadScene()
    {
        // Reload the active scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        // Optional: Ensure the time scale is 1 after reload
        Time.timeScale = 1;
    }
}