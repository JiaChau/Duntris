using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GameObject playerProjectilePrefab;
    private PlayerStats stats;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource punchSource;   // 근접 공격 사운드
    [SerializeField] private AudioSource blasterSource; // 원거리 사운드

    void Start()
    {
        stats = PlayerStatsManager.Instance.stats;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MeleeAttack();
        if (Input.GetMouseButtonDown(1))
            RangedAttack();

        // Cheat key: press backtick ` to clear all enemies
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ClearAllEnemies();
        }
    }

    void ClearAllEnemies()
    {
        BaseEnemy[] allEnemies = FindObjectsOfType<BaseEnemy>();
        Debug.Log("Clearing " + allEnemies.Length + " enemies from the room.");

        foreach (var enemy in allEnemies)
        {
            enemy.TakeDamage(9999f); // massive damage to kill any enemy
        }
    }



    /* ───────── 근접 공격 ───────── */
    void MeleeAttack()
    {
        // 사운드
        punchSource?.PlayOneShot(punchSource.clip, 1f);  // 겹쳐 재생 가능

        // 데미지 판정
        var hits = Physics.OverlapSphere(transform.position,
                                         stats.meleeRange,
                                         LayerMask.GetMask("Enemy"));

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out BaseEnemy enemy))
            {
                enemy.TakeDamage(stats.meleeDamage);
                enemy.Stun(0.3f);

                // 시각 피드백
                Debug.DrawRay(hit.transform.position, Vector3.up * 2f,
                              Color.yellow, 0.2f);
            }
        }
        Debug.Log("Player melee attack triggered.");
    }

    /* ───────── 원거리 공격 ───────── */
    void RangedAttack()
    {
        Ray   ray    = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, transform.position);

        if (!ground.Raycast(ray, out float dist)) return;

        // 사운드
        blasterSource?.PlayOneShot(blasterSource.clip, 1f);

        // 발사 방향 계산
        Vector3 target   = ray.GetPoint(dist);
        Vector3 dir      = (target - transform.position).normalized;
        Vector3 spawnPos = transform.position + dir * 0.25f;

        // 투사체 생성
        GameObject proj = Instantiate(playerProjectilePrefab, spawnPos,
                                      Quaternion.identity);

        if (proj.TryGetComponent(out Rigidbody rb))
        {
            rb.useGravity = false;
            rb.velocity   = dir * stats.projectileSpeed;
        }

        if (proj.TryGetComponent(out Projectile p))
        {
            p.damage    = stats.rangedDamage;
            p.targetTag = "Enemy";
        }
    }

    void OnDrawGizmosSelected()
    {
        if (stats == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.meleeRange);
    }
}
