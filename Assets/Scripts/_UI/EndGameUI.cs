using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

        // Set the kill and time text
        killsText.text = "Total Kills: " + kills.ToString();
        timeText.text = "Total Time: " + WaveManager.totalRunTime.ToString("F2") + "s";

        // Show the end game UI
        endGamePanel.SetActive(true);
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    public void RestartGame()
    {
        // Unpause the game
        Time.timeScale = 1f;

        // Reset game data
        WaveManager.CurrentWaveIndex = 0;
        WaveManager.RemainingWaves = 0;
        WaveManager.TotalWavesInRoom = 0;
        WaveManager.totalEnemiesKilled = 0;
        WaveManager.timeSpentInRoom = 0f;
        WaveManager.roomTime = 0f;
        WaveManager.totalRunTime = 0f;
        RoomManager.floorIndex = 0;

        // Reload game scene
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
