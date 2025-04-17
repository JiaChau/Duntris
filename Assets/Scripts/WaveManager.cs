using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    // Wave tracking for UI
    public static int CurrentWaveIndex { get; set; }
    public static int RemainingWaves { get; set; }
    public static int TotalWavesInRoom { get; set; }


    public static float roomTime = 0f;
    public static float totalRunTime = 0f;


    [Header("Enemy Prefabs")]
    public GameObject meleePrefab;
    public GameObject throwerPrefab;

    [Header("Optional Delay After Room Load")]
    public float delayBeforeStart = 0.5f;

    [System.Serializable]
    public class Wave
    {
        public int numMelee;
        public int numThrowers;
        public float spawnRate = 0.5f;
    }

    [Header("Wave Definitions")]
    public Wave[] waves;

    [Header("UI References")]
    public TMP_Text waveClearedText;
    public TMP_Text finalWaveText;
    public TMP_Text exitUnlockedText;

    private Transform[] spawnPoints;
    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool spawning = false;
    private bool waveFullySpawned = false;

    public static int totalEnemiesKilled = 0;
    public static float timeSpentInRoom = 0f;

    private RoomExit roomExit;

    void Start()
    {
        timeSpentInRoom = 0f;
        roomTime = 0f; // Reset room timer

        // Find ExitBeam in room
        Transform exitBeamTransform = transform.Find("ExitBeam");
        if (exitBeamTransform != null)
        {
            roomExit = exitBeamTransform.GetComponent<RoomExit>();
            if (roomExit != null)
            {
                roomExit.HideExit();
            }
            else
            {
                Debug.LogError("ExitBeam found, but RoomExit script is missing!");
            }
        }
        else
        {
            Debug.LogError("ExitBeam object not found as child of room prefab!");
        }

        // Find spawn points
        List<Transform> validSpawns = new List<Transform>();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("SpawnPoint"))
                validSpawns.Add(t);
        }

        spawnPoints = validSpawns.ToArray();

        if (spawnPoints.Length == 0)
            Debug.LogWarning("No spawn points tagged as 'SpawnPoint' found in room!");

        StartCoroutine(StartFirstWave());
    }

    void Update()
    {
        timeSpentInRoom += Time.deltaTime;
        totalRunTime += Time.deltaTime;
        roomTime += Time.deltaTime;


        if (waveFullySpawned && enemiesAlive == 0 && !spawning)
        {
            if (currentWave < waves.Length - 1)
            {
                currentWave++;
                RemainingWaves = Mathf.Max(waves.Length - currentWave, 0);
                CurrentWaveIndex = waves.Length - 1;

                StartCoroutine(SpawnWave());
                StartCoroutine(ShowPopupSequence(new string[] { "Wave Cleared!" }));
            }
            else
            {
                // Last wave just completed
                RemainingWaves = 0;
                CurrentWaveIndex = waves.Length - 1;

                Debug.Log("All waves cleared.");

                if (roomExit != null)
                {
                    roomExit.UnlockExit();
                }

                HUD hud = FindObjectOfType<HUD>();
                if (hud != null)
                {
                    hud.StopTimer();
                }

                StartCoroutine(ShowPopupSequence(new string[]
                {
            "Final Wave Cleared!",
            "Exit Unlocked!",
            "Room cleared in " + timeSpentInRoom.ToString("F2") + " seconds",
            "Enemies defeated: " + totalEnemiesKilled
                }));
            }

            waveFullySpawned = false;
        }
    }

    IEnumerator StartFirstWave()
    {
        RemainingWaves = waves.Length;
        TotalWavesInRoom = waves.Length;
        CurrentWaveIndex = 0;

        yield return new WaitForSeconds(delayBeforeStart);
        yield return StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        if (currentWave >= waves.Length || spawning) yield break;

        spawning = true;
        Wave wave = waves[currentWave];

        for (int i = 0; i < wave.numMelee; i++)
        {
            SpawnEnemy(meleePrefab);
            yield return new WaitForSeconds(wave.spawnRate);
        }

        for (int i = 0; i < wave.numThrowers; i++)
        {
            SpawnEnemy(throwerPrefab);
            yield return new WaitForSeconds(wave.spawnRate);
        }

        waveFullySpawned = true;
        spawning = false;
    }

    void SpawnEnemy(GameObject prefab)
    {
        if (spawnPoints.Length == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);

        enemiesAlive++;

        BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            baseEnemy.OnDeath += HandleEnemyDeath;
        }
    }

    void HandleEnemyDeath()
    {
        enemiesAlive--;
        totalEnemiesKilled++;
    }

    IEnumerator ShowPopupSequence(string[] messages)
    {
        TMP_Text[] popupTargets = new TMP_Text[]
        {
            finalWaveText,
            exitUnlockedText,
            waveClearedText
        };

        int popupIndex = 0;

        foreach (string message in messages)
        {
            TMP_Text target = popupTargets[Mathf.Min(popupIndex, popupTargets.Length - 1)];
            target.text = message;
            target.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.75f);

            target.gameObject.SetActive(false);
            popupIndex++;
        }
    }
}
