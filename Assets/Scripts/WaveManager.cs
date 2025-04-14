using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public GameObject meleePrefab;
    public GameObject throwerPrefab;
    public Transform[] spawnPoints;

    [System.Serializable]
    public class Wave
    {
        public int numMelee;
        public int numThrowers;
        public float spawnRate;
    }

    public Wave[] waves;
    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
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

        currentWave = Mathf.Min(currentWave + 1, waves.Length - 1);
    }

    void SpawnEnemy(GameObject prefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
