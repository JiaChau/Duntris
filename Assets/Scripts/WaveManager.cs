using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic; // Add this to use List<T>


public class WaveManager : MonoBehaviour
{
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

    private Transform[] spawnPoints;
    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool spawning = false;
    private bool waveFullySpawned = false;

    public static int totalEnemiesKilled = 0;
    public static float timeSpentInRoom = 0f;

    private RoomExit roomExit;

    [Header("UI References")]
    public TMP_Text waveClearedText;
    public TMP_Text finalWaveText;
    public TMP_Text exitUnlockedText;

    void Start()
    {
        timeSpentInRoom = 0f;

        // Find the ExitBeam GameObject manually
        Transform exitBeamTransform = transform.Find("ExitBeam");
        if (exitBeamTransform != null)
        {
            roomExit = exitBeamTransform.GetComponent<RoomExit>();
            if (roomExit != null)
            {
                roomExit.HideExit(); // Hide exit at start
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

        if (waveFullySpawned && enemiesAlive == 0 && !spawning)
        {
            currentWave++;

            if (currentWave < waves.Length)
            {
                StartCoroutine(SpawnWave());
                ShowWaveClearedPopup(); // Show "Wave Cleared" message
            }
            else
            {
                Debug.Log("All waves cleared.");

                if (roomExit != null)
                {
                    roomExit.UnlockExit(); // Unlocks and activates the beam
                    ShowExitUnlockedMessage(); // Show "Exit Unlocked" message
                }

                ShowFinalWaveMessage(); // Show "Final Wave!" message
                Debug.Log("Room cleared in " + timeSpentInRoom.ToString("F2") + " seconds.");
                Debug.Log("Enemies defeated so far: " + totalEnemiesKilled);
            }

            waveFullySpawned = false;
        }
    }

    IEnumerator StartFirstWave()
    {
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

    // Show the "Wave Cleared" message
    void ShowWaveClearedPopup()
    {
        waveClearedText.gameObject.SetActive(true);
        waveClearedText.text = "Wave Cleared!";
        StartCoroutine(HideWaveMessage(waveClearedText));
    }

    // Show the "Final Wave!" message
    void ShowFinalWaveMessage()
    {
        finalWaveText.gameObject.SetActive(true);
        finalWaveText.text = "Final Wave!";
        StartCoroutine(HideWaveMessage(finalWaveText));
    }

    // Show the "Exit Unlocked" message
    void ShowExitUnlockedMessage()
    {
        exitUnlockedText.gameObject.SetActive(true);
        exitUnlockedText.text = "Exit Unlocked!";
        StartCoroutine(HideWaveMessage(exitUnlockedText));
    }

    // Hide the message after 2 seconds
    IEnumerator HideWaveMessage(TMP_Text message)
    {
        yield return new WaitForSeconds(2f);
        message.gameObject.SetActive(false);
    }
}
