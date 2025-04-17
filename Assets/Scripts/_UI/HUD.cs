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
    private bool isTimerActive = false;
    public Canvas hudCanvas; // assign in Inspector



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
        healthBarSlider.value = currentHealth;
    }

    void UpdateKillCount()
    {
        killsText.text = "Kills: " + WaveManager.totalEnemiesKilled.ToString();
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
        roomLabelText.text = "Floor: " + (RoomManager.floorIndex + 1); // Correct source for floor number
    }

    public void EnableRoomTimer(bool enabled)
    {
        isTimerActive = enabled;

        // Set placeholder text if timer is off
        if (!enabled)
        {
            timerText.text = "Time: --.--";
        }

        // Toggle the whole HUD canvas visibility
        if (hudCanvas != null)
        {
            hudCanvas.enabled = enabled;
        }
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }


}
