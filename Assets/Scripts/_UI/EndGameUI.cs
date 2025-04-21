using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text killsText;
    public TMP_Text timeText;
    public TMP_Text tetrisScoreText; // Optional: assign in Inspector
    public GameObject endGamePanel;
    public GameObject restartButton;
    public GameObject mainMenuButton;

    public static bool IsGameEnded = false;

    void Start()
    {
        endGamePanel.SetActive(false);  // Hide at start
    }

    public void ShowEndScreen()
    {
        IsGameEnded = true;
        GameUIManager.IsUIOpen = true;
        Time.timeScale = 0f;

        int kills = WaveManager.totalEnemiesKilled;
        float time = WaveManager.totalRunTime;
        int tetrisScore = BlockGrid.GetCumulativeScore();

        killsText.text = "Total Kills: " + kills;
        timeText.text = "Total Time: " + time.ToString("F2") + "s";

        if (tetrisScoreText != null)
            tetrisScoreText.text = "Tetris Score: " + tetrisScore;

        endGamePanel.SetActive(true);
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        IsGameEnded = false;
        GameUIManager.IsUIOpen = false;

        ResetGameStats();
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsGameEnded = false;
        GameUIManager.IsUIOpen = false;

        ResetGameStats();
        SceneManager.LoadScene("Menu");
    }

    private void ResetGameStats()
    {
        WaveManager.CurrentWaveIndex = 0;
        WaveManager.RemainingWaves = 0;
        WaveManager.TotalWavesInRoom = 0;
        WaveManager.totalEnemiesKilled = 0;
        WaveManager.timeSpentInRoom = 0f;
        WaveManager.roomTime = 0f;
        WaveManager.totalRunTime = 0f;
        RoomManager.floorIndex = 0;
        BlockGrid.cumulativeScore = 0;

        if (BlockPlacementManager.Instance != null)
            BlockPlacementManager.Instance.ClearSelected();
    }
}
