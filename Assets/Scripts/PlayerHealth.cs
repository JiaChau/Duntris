using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    public float currentHealth;
    private PlayerStats stats;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource hurtSource;
    [SerializeField] private AudioSource healSource;
    [SerializeField] private AudioSource deadSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        stats         = PlayerStatsManager.Instance.stats;
        currentHealth = stats.maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (hurtSource) hurtSource.PlayOneShot(hurtSource.clip, 0.5f);

        currentHealth -= amount;
        RepelEnemies();

        if (currentHealth <= 0) GameOver();
    }

    void RepelEnemies()
    {
        float repelRadius = 5f;
        float repelForce  = 10f;
        var enemies = Physics.OverlapSphere(transform.position, repelRadius, LayerMask.GetMask("Enemy"));

        foreach (var enemy in enemies)
        {
            Vector3 dir = (enemy.transform.position - transform.position).normalized;
            enemy.GetComponent<Rigidbody>()?.AddForce(dir * repelForce, ForceMode.Impulse);
        }
    }

    public void Heal(float amount, bool increaseMax = false)
    {
        if (healSource) healSource.PlayOneShot(healSource.clip, 1f);

        if (increaseMax) stats.maxHealth += amount;

        currentHealth  = Mathf.Min(currentHealth + amount, stats.maxHealth);
    }

    void GameOver()
    {
        if (deadSource) deadSource.PlayOneShot(deadSource.clip, 2f);

        ApplyStatPenalty();

        if (FindObjectOfType<EndGameUI>() is EndGameUI endUI)
            endUI.ShowEndScreen();
        else
            Debug.LogWarning("EndGameUI not found in scene.");

        // Stop the HUD timer
        FindObjectOfType<HUD>()?.StopTimer();

        Time.timeScale = 0f;
    }


    void ApplyStatPenalty()
    {
        var rand = new System.Random();

        void Reduce(ref float stat, float baseVal, float maxLoss)
        {
            float penalty = (float)(rand.NextDouble() * maxLoss);
            stat = Mathf.Max(stat - penalty, baseVal);
        }

        void ReduceInt(ref int stat, int baseVal, int maxLoss)
        {
            int penalty = rand.Next(1, maxLoss + 1);
            stat = Mathf.Max(stat - penalty, baseVal);
        }

        Reduce(ref stats.maxHealth, stats.baseHealth, 30f);
        Reduce(ref stats.moveSpeed, stats.baseSpeed, 1f);
        Reduce(ref stats.meleeDamage, stats.baseMeleeDamage, 5f);
        Reduce(ref stats.meleeRange, stats.baseMeleeRange, 1f);
        Reduce(ref stats.rangedDamage, stats.baseRangedDamage, 5f);
        Reduce(ref stats.projectileSpeed, stats.baseProjectileSpeed, 3f);
        ReduceInt(ref stats.projectileCount, stats.baseProjectileCount, 2);
    }
}
