using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinishGameUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text congratsText;
    public TMP_Text killsText;
    public TMP_Text timeText;
    public TMP_Text scoreText;

    public GameObject finishPanel;
    public GameObject restartButton;
    public GameObject mainMenuButton;

    public static bool IsGameFinished = false;

    void Start()
    {
        finishPanel.SetActive(false); // Hide UI initially
    }

    public void ShowFinishScreen()
    {
        IsGameFinished = true;
        GameUIManager.IsUIOpen = true;
        Time.timeScale = 0f;

        int kills = WaveManager.totalEnemiesKilled;
        float totalTime = WaveManager.totalRunTime;
        int tetrisScore = BlockGrid.GetCumulativeScore();

        congratsText.text = "Dungeon Complete!";
        killsText.text = "Total Kills: " + kills;
        timeText.text = "Total Time: " + totalTime.ToString("F2") + "s";
        scoreText.text = "Tetris Score: " + tetrisScore;

        finishPanel.SetActive(true);
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ResetGlobalStats();
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        ResetGlobalStats();
        SceneManager.LoadScene("Menu");
    }

    void ResetGlobalStats()
    {
        IsGameFinished = false;
        GameUIManager.IsUIOpen = false;

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
