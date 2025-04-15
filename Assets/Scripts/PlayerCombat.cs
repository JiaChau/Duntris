using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GameObject playerProjectilePrefab;
    private PlayerStats stats;
    [SerializeField] private AudioClip punchSoundClip;
    [SerializeField] private AudioClip blasterSoundClip;

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
    }

    void MeleeAttack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, stats.meleeRange, LayerMask.GetMask("Enemy"));
        //audio
        AudioSource.PlayClipAtPoint(punchSoundClip, transform.position, 1f);

        foreach (var hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy != null)
            {

                enemy.TakeDamage(stats.meleeDamage);
                // Optional: visual indicator
                Debug.DrawRay(hit.transform.position, Vector3.up * 2f, Color.yellow, 0.2f); // flashes upward line on hit

                enemy.Stun(0.3f); // Half-second stun
            }
        }

        Debug.Log("Player melee attack triggered.");

        // Optional: Visual effect, animation, or sound
    }


    void RangedAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, transform.position);
        AudioSource.PlayClipAtPoint(blasterSoundClip, transform.position, 1f);

        if (ground.Raycast(ray, out float dist))
        {
            Vector3 target = ray.GetPoint(dist);
            Vector3 dir = (target - transform.position).normalized;

            Vector3 spawnPos = transform.position + dir * 0.25f; // closer, no height

            GameObject proj = Instantiate(playerProjectilePrefab, spawnPos, Quaternion.identity);
            Rigidbody rb = proj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = false;
                rb.velocity = dir * stats.projectileSpeed;
            }

            Projectile p = proj.GetComponent<Projectile>();
            if (p != null)
            {
                p.damage = stats.rangedDamage;
                p.targetTag = "Enemy";
            }

        }
    }

    void OnDrawGizmosSelected()
    {
        if (stats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stats.meleeRange);
        }
    }

}
