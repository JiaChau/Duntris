using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Importing TextMeshPro namespace

public class HUD : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text killsText;  // Using TMP_Text for TextMeshPro
    public TMP_Text waveIndicatorText;  // Using TMP_Text for TextMeshPro
    public TMP_Text timerText;  // Using TMP_Text for TextMeshPro
    public TMP_Text roomLabelText;  // Using TMP_Text for TextMeshPro
    public Slider healthBarSlider;

    private void Update()
    {
        UpdateHealthBar();
        UpdateKillCount();
        UpdateWaveIndicator();
        UpdateTimer();
        UpdateRoomLabel();
    }

        // Update health bar based on player's current health
    void UpdateHealthBar()
    {
        float currentHealth = PlayerHealth.Instance.currentHealth;  // Now using currentHealth
        healthBarSlider.value = currentHealth;  // Adjust slider based on current health
    }


    // Update kill count
    void UpdateKillCount()
    {
        killsText.text = "Kills: " + WaveManager.totalEnemiesKilled.ToString();
    }

    // Update wave indicator (for example, 'Wave 1' or 'Wave 2')
    void UpdateWaveIndicator()
    {
        waveIndicatorText.text = "Wave: " + (WaveManager.totalEnemiesKilled / 5 + 1).ToString(); // Update based on how your waves are handled
    }

    // Update the timer with time spent in the current room
    void UpdateTimer()
    {
        timerText.text = "Time: " + WaveManager.timeSpentInRoom.ToString("F2") + "s"; // Example with 2 decimals
    }

    // Update room/floor label
    void UpdateRoomLabel()
    {
        roomLabelText.text = "Floor: " + (WaveManager.totalEnemiesKilled / 5 + 1).ToString(); // Example, adjust based on your game
    }
}
