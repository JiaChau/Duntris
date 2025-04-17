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

    // Define unique values in the enum
    private enum RoomType { Combat1, Combat2, Healing, Combat3, Treasure, Combat4, Healing2, Combat5, Crafting, Tetris }

    // Room pattern (to control room order)
    private RoomType[] roomPattern =
    {
        RoomType.Combat1, RoomType.Combat2, RoomType.Healing,
        RoomType.Combat3, RoomType.Treasure, RoomType.Combat4,
        RoomType.Healing2, RoomType.Combat5, RoomType.Crafting,
        RoomType.Tetris
    };

    private void Start()
    {
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
        {
            Destroy(currentRoom);
        }

        SetThemeForFloor();

        RoomType roomType = roomPattern[currentFloor % roomPattern.Length];
        GameObject selectedRoomPrefab = GetRoomPrefabByType(roomType);

        currentRoom = Instantiate(selectedRoomPrefab, Vector3.zero, Quaternion.identity);

        PositionPlayer();
        HandleExitVisibility(roomType);

        floorIndex = currentFloor;
        Debug.Log($"Entering Floor {floorIndex}: {roomType}");
        currentFloor++;

        // Timer control logic
        HUD hud = FindObjectOfType<HUD>();
        if (hud != null)
        {
            bool isCombatRoom = roomType == RoomType.Combat1 || roomType == RoomType.Combat2 ||
                                roomType == RoomType.Combat3 || roomType == RoomType.Combat4 ||
                                roomType == RoomType.Combat5;

            hud.EnableRoomTimer(isCombatRoom);
        }

        // Show Buff UI if in a buff (crafting) room
        BuffStatsUI buffUI = FindObjectOfType<BuffStatsUI>();
        if (buffUI != null)
        {
            bool isBuffRoom = roomType == RoomType.Crafting;
            buffUI.ShowStatsPanel(isBuffRoom);

            if (isBuffRoom)
            {
                buffUI.UpdateStats(PlayerStatsManager.Instance.stats);
            }
        }
    }



    private GameObject GetRoomPrefabByType(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.Combat1:
            case RoomType.Combat2:
            case RoomType.Combat3:
            case RoomType.Combat4:
            case RoomType.Combat5:
                return currentTheme.combatRoom;

            case RoomType.Healing:
            case RoomType.Healing2:
                return currentTheme.healingRoom;

            case RoomType.Treasure:
                return currentTheme.treasureRoom;

            case RoomType.Crafting:
                return currentTheme.craftingRoom;

            case RoomType.Tetris:
                return currentTheme.tetrisRoom;

            default:
                Debug.LogError("Room type not found!");
                return null;
        }
    }

    private void PositionPlayer()
    {
        Transform entryPoint = currentRoom.transform.Find("EmptyWaypoint");

        if (entryPoint != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = entryPoint.position;
            }
        }
        else
        {
            Debug.LogWarning("EmptyWaypoint not found in the room!");
        }
    }

    private void HandleExitVisibility(RoomType roomType)
    {
        GameObject exitPoint = currentRoom.transform.Find("ExitBeam")?.gameObject;

        if (exitPoint != null)
        {
            RoomExit exitScript = exitPoint.GetComponent<RoomExit>();

            if (roomType == RoomType.Combat1 || roomType == RoomType.Combat2 || roomType == RoomType.Combat3 ||
                roomType == RoomType.Combat4 || roomType == RoomType.Combat5 || roomType == RoomType.Tetris)
            {
                exitScript.HideExit(); // Hide exit initially
            }
            else
            {
                exitScript.UnlockExit(); // Show exit immediately
            }
        }
        else
        {
            Debug.LogWarning("ExitBeam not found in the room!");
        }
    }

    // Temporary method to simulate combat/tetris completion
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject exitPoint = currentRoom.transform.Find("ExitBeam")?.gameObject;
            if (exitPoint != null)
            {
                RoomExit exitScript = exitPoint.GetComponent<RoomExit>();
                exitScript.UnlockExit();
                Debug.Log("Test: Exit unlocked manually!");
            }
        }
    }
}
