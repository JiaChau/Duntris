using UnityEngine;
using UnityEngine.AI;

public class ThrowerEnemy : BaseEnemy
{
    public GameObject enemyProjectilePrefab;
    public float throwCooldown = 2f;
    public float preferredDistance = 10f;
    private float throwTimer;

    protected override void Update()
    {
        base.Update(); // Ensure stun logic runs

        if (!player || agent.isStopped) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < preferredDistance - 1f)
        {
            Vector3 fleeDir = (transform.position - player.position).normalized;
            Vector3 rawFleeTarget = transform.position + fleeDir * preferredDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(rawFleeTarget, out hit, 3f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                agent.SetDestination(transform.position + fleeDir * 1f);
            }
        }
        else if (distance > preferredDistance + 1f)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }

        if (throwTimer <= 0f && distance <= preferredDistance + 2f)
        {
            ThrowProjectile();
            throwTimer = throwCooldown;
        }

        throwTimer -= Time.deltaTime;
    }

    void ThrowProjectile()
    {
        Vector3 toPlayer = (player.position - transform.position);
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;
        Vector3 direction = toPlayer.normalized;

        float spreadSize = Mathf.Clamp(distance * 0.1f, 0.25f, 1.5f);
        float heightOffset = Mathf.Clamp(distance * 0.1f, 0.5f, 2f);

        Vector3 baseSpawn = transform.position + direction * 0.75f + Vector3.up * heightOffset;

        Vector3 right = Vector3.Cross(Vector3.up, direction);
        Vector3[] offsets = new Vector3[]
        {
            Vector3.zero,
            right * spreadSize,
            -right * spreadSize,
            Vector3.forward * spreadSize,
            -Vector3.forward * spreadSize
        };

        foreach (var offset in offsets)
        {
            Vector3 spawnPos = baseSpawn + offset;
            SpawnEnemyProjectile(spawnPos, direction);
        }
    }

    void SpawnEnemyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        GameObject proj = Instantiate(enemyProjectilePrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = true;
            rb.velocity = direction.normalized * 10f;
        }

        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.damage = damage;
            p.targetTag = "Player";
            p.lifetime = 5f;
        }
    }
}
