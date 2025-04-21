using UnityEngine;
using UnityEngine.UI;

public class TetrisGameManager : MonoBehaviour
{
    public static TetrisGameManager Instance;
    public static bool IsPlayingTetris = false;

    [Header("Tetris UI")]
    public GameObject tetrisCanvas;       // The full BlockBlast UI

    [Header("Player UI to Hide")]
    public GameObject hudCanvas;          // Wave / floor UI
    public GameObject healthCanvas;       // Health bar UI

    [Header("Buttons")]
    public Button confirmButton;

    [HideInInspector]
    public RoomExit currentRoomExit;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (confirmButton != null)
            confirmButton.onClick.AddListener(EndSession);
    }

    public void StartSession()
    {
        IsPlayingTetris = true;
        GameUIManager.IsUIOpen = true; // Mark UI as open

        if (tetrisCanvas != null)
        {
            tetrisCanvas.SetActive(true);
            Debug.Log("[TETRIS] Canvas enabled.");
        }

        if (hudCanvas != null) hudCanvas.SetActive(false);
        if (healthCanvas != null) healthCanvas.SetActive(false);

        Time.timeScale = 0f;
        Debug.Log("[TETRIS] Gameplay paused.");
    }

    public void EndSession()
    {
        tetrisCanvas.SetActive(false);
        IsPlayingTetris = false;
        GameUIManager.IsUIOpen = false; // Allow combat/movement again

        if (hudCanvas != null) hudCanvas.SetActive(true);
        if (healthCanvas != null) healthCanvas.SetActive(true);

        Time.timeScale = 1f;
        Debug.Log("[TETRIS] Session ended.");

        BlockGrid grid = FindObjectOfType<BlockGrid>();
        if (grid != null)
            grid.ClearGrid();

        if (BlockPlacementManager.Instance != null)
            BlockPlacementManager.Instance.ClearSelected();

        if (currentRoomExit != null)
        {
            currentRoomExit.gameObject.SetActive(true);
            currentRoomExit.UnlockExit();
            Debug.Log("[TETRIS] ExitBeam activated.");
        }
        else
        {
            Debug.LogWarning("[TETRIS] No RoomExit linked.");
        }
    }
}
