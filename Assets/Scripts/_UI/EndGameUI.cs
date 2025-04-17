using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // For loading scenes

public class EndGameUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text killsText;
    public TMP_Text timeText;
    public GameObject endGamePanel;
    public GameObject restartButton;
    public GameObject mainMenuButton;

    void Start()
    {
        endGamePanel.SetActive(false);  // Hide the end game UI at start
    }

    public void ShowEndScreen()
    {
        // Get the data from the WaveManager
        int kills = WaveManager.totalEnemiesKilled;
        float roomTime = WaveManager.timeSpentInRoom;

        // Set the kill and time text
        killsText.text = "Total Kills: " + kills.ToString();
        timeText.text = "Time in Room: " + roomTime.ToString("F2") + "s";  // Format time to 2 decimal places

        // Activate the end game UI panel
        endGamePanel.SetActive(true);

        // Optional: Show the restart and main menu buttons
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);

        // Reset global counter for new run
        WaveManager.totalEnemiesKilled = 0;
    }

    // Restart the current scene
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");  // Reload the current scene
    }

    // Load the main menu scene (adjust as needed)
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");  // Replace "MainMenu" with the actual name of your main menu scene
    }
}
