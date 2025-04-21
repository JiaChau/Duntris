using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text killsText;
    public TMP_Text waveIndicatorText;
    public TMP_Text timerText;
    public TMP_Text roomLabelText;

    public Slider healthBarSlider;
    public TMP_Text healthText; // Add this in the inspector

    private bool isTimerActive = false;
    public Canvas hudCanvas;

    private void Update()
    {
        UpdateHealthBar();
        UpdateKillCount();
        UpdateWaveIndicator();
        UpdateTimer();
        UpdateRoomLabel();
    }

    void UpdateHealthBar()
    {
        float currentHealth = PlayerHealth.Instance.currentHealth;
        float maxHealth = PlayerStatsManager.Instance.stats.maxHealth;

        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;

        if (healthText != null)
            healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
    }

    void UpdateKillCount()
    {
        killsText.text = "Kills: " + WaveManager.totalEnemiesKilled;
    }

    void UpdateWaveIndicator()
    {
        waveIndicatorText.text = "Waves: " + (WaveManager.CurrentWaveIndex + 1) + " / " + WaveManager.TotalWavesInRoom;
    }

    void UpdateTimer()
    {
        if (isTimerActive)
        {
            timerText.text = "Time: " + WaveManager.roomTime.ToString("F2") + "s";
        }
    }

    void UpdateRoomLabel()
    {
        roomLabelText.text = "Floor: " + (RoomManager.floorIndex + 1);
    }

    public void EnableRoomTimer(bool enabled)
    {
        isTimerActive = enabled;
        if (!enabled) timerText.text = "Time: --.--";
        if (hudCanvas != null) hudCanvas.enabled = enabled;
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }
}
