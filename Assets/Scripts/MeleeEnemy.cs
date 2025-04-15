using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float attackTimer;
    [SerializeField] private AudioClip punchSoundClip;

    protected override void Update()
    {
        base.Update(); // Ensure stun logic runs

        if (!player || agent.isStopped) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange - 0.5f)
        {
            Vector3 toPlayer = (player.position - transform.position).normalized;
            Vector3 stopPoint = player.position - toPlayer * (attackRange - 0.2f);
            agent.SetDestination(stopPoint);
            
        }
        else
        {
            agent.SetDestination(transform.position);
        }

        if (distance <= attackRange && attackTimer <= 0f)
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            attackTimer = attackCooldown;
            AudioSource.PlayClipAtPoint(punchSoundClip, transform.position, 0.1f);
        }

        attackTimer -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}
