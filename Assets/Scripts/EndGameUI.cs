using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [Header("UI References")]
    public Text killsText;
    public Text timeText;
    public GameObject endGamePanel;

    void Start()
    {
        endGamePanel.SetActive(false);
    }

    public void ShowEndScreen()
    {
        int kills = WaveManager.totalEnemiesKilled;
        float roomTime = WaveManager.timeSpentInRoom;

        killsText.text = "Total Kills: " + kills;
        timeText.text = "Time in Room: " + roomTime.ToString("F2") + "s";

        endGamePanel.SetActive(true);

        // Optional: reset global counter when starting a new run
        WaveManager.totalEnemiesKilled = 0;
    }
}
