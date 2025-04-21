using UnityEngine;
using UnityEngine.SceneManagement;   // For scene transitions
#if UNITY_EDITOR
using UnityEditor;                  // For quitting play mode in editor
#endif

public class MainMenuUI : MonoBehaviour
{
    // “Play” 버튼에서 호출
    public void PlayGame()
    {
        // Ensure time isn't frozen from previous game over
        Time.timeScale = 1f;

        // Reset static data from previous run
        WaveManager.CurrentWaveIndex = 0;
        WaveManager.RemainingWaves = 0;
        WaveManager.TotalWavesInRoom = 0;
        WaveManager.totalEnemiesKilled = 0;
        WaveManager.timeSpentInRoom = 0f;
        WaveManager.roomTime = 0f;
        WaveManager.totalRunTime = 0f;

        RoomManager.floorIndex = 0;
        BlockGrid.cumulativeScore = 0; // ✅ Reset Tetris score when starting new game

        // Load game scene (replace with actual gameplay scene name)
        SceneManager.LoadScene("Game");
    }

    // “Quit” 버튼에서 호출
    public void QuitGame()
    {
        Application.Quit(); // For builds
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Stop play mode in editor
#endif
    }
}
