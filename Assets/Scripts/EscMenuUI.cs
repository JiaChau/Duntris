using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenuUI : MonoBehaviour
{
    public GameObject escMenuPanel;
    private bool isPaused = false;

    void Start()
    {
        escMenuPanel.SetActive(false);
        GameUIManager.IsUIOpen = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Prevent ESC if finish UI or end UI is active
        if (FinishGameUI.IsGameFinished || EndGameUI.IsGameEnded) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isPaused = !isPaused;
        escMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        // Set UI state
        GameUIManager.IsUIOpen = isPaused;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        GameUIManager.IsUIOpen = false;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
