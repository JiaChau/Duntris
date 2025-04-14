using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public PlayerStats stats;

    public static PlayerStatsManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (stats == null)
        {
            stats = new PlayerStats();
        }
    }
}
