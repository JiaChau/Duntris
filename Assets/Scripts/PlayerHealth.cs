using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    private PlayerStats stats;

    [SerializeField] private AudioClip hurtSoundClip;
    [SerializeField] private AudioClip HealSoundClip;
    [SerializeField] private AudioClip deadSoundClip;

    void Start()
    {
        stats = PlayerStatsManager.Instance.stats;
        currentHealth = stats.maxHealth;
    }

    public void TakeDamage(float amount)
    {
        AudioSource.PlayClipAtPoint(hurtSoundClip, transform.position, 0.5f);

        currentHealth -= amount;
        RepelEnemies();

        if (currentHealth <= 0)
            GameOver();
    }

    void RepelEnemies()
    {
        float repelRadius = 5f;
        float repelForce = 10f;
        Collider[] enemies = Physics.OverlapSphere(transform.position, repelRadius, LayerMask.GetMask("Enemy"));

        foreach (var enemy in enemies)
        {
            Vector3 dir = (enemy.transform.position - transform.position).normalized;
            enemy.GetComponent<Rigidbody>()?.AddForce(dir * repelForce, ForceMode.Impulse);
        }
    }

    public void Heal(float amount, bool increaseMax = false)
    {
        AudioSource.PlayClipAtPoint(HealSoundClip, transform.position, 1f);

        if (increaseMax)
            stats.maxHealth += amount;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, stats.maxHealth);
    }

    void GameOver()
    {
        AudioSource.PlayClipAtPoint(deadSoundClip, transform.position, 2f);

        ApplyStatPenalty();

        // Show new end screen instead of old game over UI
        EndGameUI endUI = FindObjectOfType<EndGameUI>();
        if (endUI != null)
        {
            endUI.ShowEndScreen();
        }
        else
        {
            Debug.LogWarning("EndGameUI not found in scene.");
        }

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
