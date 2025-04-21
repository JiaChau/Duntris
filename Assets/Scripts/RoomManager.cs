using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static int floorIndex = 0;

    [System.Serializable]
    public class RoomTheme
    {
        public GameObject combatRoom;
        public GameObject healingRoom;
        public GameObject treasureRoom;
        public GameObject craftingRoom;
        public GameObject tetrisRoom;
    }

    public RoomTheme dungeonTheme;
    public RoomTheme arcadeTheme;
    public RoomTheme marketTheme;

    private RoomTheme currentTheme;
    private int currentFloor = 0;
    private GameObject currentRoom;

    private enum RoomType { Combat1, Combat2, Healing, Combat3, Crafting, Combat4, Healing2, Combat5, Treasure, Tetris }

    private RoomType[] roomPattern =
    {
        RoomType.Combat1, RoomType.Combat2, RoomType.Healing,
        RoomType.Combat3, RoomType.Crafting, RoomType.Combat4,
        RoomType.Healing2, RoomType.Combat5, RoomType.Treasure,
        RoomType.Tetris
    };

    private void Start()
    {
#if UNITY_EDITOR
          currentFloor = 28;
#endif
        WaveManager.totalRunTime = 0f;

        SetThemeForFloor();
        LoadNextRoom();
    }

    private void SetThemeForFloor()
    {
        if (currentFloor < 10) currentTheme = dungeonTheme;
        else if (currentFloor < 20) currentTheme = arcadeTheme;
        else currentTheme = marketTheme;
    }

    public void LoadNextRoom()
    {
        if (currentRoom != null)
            Destroy(currentRoom);

        SetThemeForFloor();

        RoomType roomType = roomPattern[currentFloor % roomPattern.Length];
        GameObject selectedRoomPrefab = GetRoomPrefabByType(roomType);
        currentRoom = Instantiate(selectedRoomPrefab, Vector3.zero, Quaternion.identity);

        PositionPlayer();
        RoomExit exitScript = HandleExitVisibility(roomType);

        floorIndex = currentFloor;
        Debug.Log($"Entering Floor {floorIndex}: {roomType}");
        if (floorIndex < 29) currentFloor++;

        HUD hud = FindObjectOfType<HUD>();
        if (hud != null)
        {
            bool isCombatRoom = roomType.ToString().Contains("Combat");
            hud.EnableRoomTimer(isCombatRoom);
        }

        BuffStatsUI buffUI = FindObjectOfType<BuffStatsUI>();
        if (buffUI != null)
        {
            bool isCraftingRoom = roomType == RoomType.Crafting;

            buffUI.ShowStatsPanel(isCraftingRoom);

            if (isCraftingRoom)
                buffUI.UpdateStats(PlayerStatsManager.Instance.stats);
        }


        if (roomType == RoomType.Tetris && TetrisGameManager.Instance != null)
        {
            TetrisGameManager.Instance.currentRoomExit = exitScript;
            if (exitScript != null) exitScript.gameObject.SetActive(false);
            TetrisGameManager.Instance.StartSession();
        }
    }

    private GameObject GetRoomPrefabByType(RoomType roomType)
    {
        return roomType switch
        {
            RoomType.Combat1 or RoomType.Combat2 or RoomType.Combat3 or RoomType.Combat4 or RoomType.Combat5 => currentTheme.combatRoom,
            RoomType.Healing or RoomType.Healing2 => currentTheme.healingRoom,
            RoomType.Treasure => currentTheme.treasureRoom,
            RoomType.Crafting => currentTheme.craftingRoom,
            RoomType.Tetris => currentTheme.tetrisRoom,
            _ => null,
        };
    }

    private void PositionPlayer()
    {
        Transform entryPoint = currentRoom.transform.Find("EmptyWaypoint");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (entryPoint != null && player != null)
        {
            player.transform.position = entryPoint.position;
        }
        else
        {
            Debug.LogWarning("Missing EmptyWaypoint or Player!");
        }
    }

    private RoomExit HandleExitVisibility(RoomType roomType)
    {
        GameObject exitObj = currentRoom.transform.Find("ExitBeam")?.gameObject;
        RoomExit exitScript = exitObj?.GetComponent<RoomExit>();

        if (exitScript != null)
        {
            bool isLockedRoom = roomType.ToString().Contains("Combat") || roomType == RoomType.Tetris;
            if (isLockedRoom) exitScript.HideExit();
            else exitScript.UnlockExit();
        }

        return exitScript;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject exitObj = currentRoom.transform.Find("ExitBeam")?.gameObject;
            RoomExit exitScript = exitObj?.GetComponent<RoomExit>();
            exitScript?.UnlockExit();
            Debug.Log("Debug: Manually unlocked exit.");
        }
    }
}
