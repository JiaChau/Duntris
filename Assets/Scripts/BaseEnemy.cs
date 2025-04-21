using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float moveSpeed = 3.5f;

    protected Transform player;
    protected NavMeshAgent agent;

    private bool isStunned = false;
    private float stunTimer = 0f;

    public event System.Action OnDeath;

    public virtual void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;

        if (agent != null && agent.isOnNavMesh)
            agent.isStopped = true;
    }

    protected virtual void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                agent.isStopped = false;
            }
        }
    }

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        if (BlockInventoryUI.Instance != null && BlockInventory.Instance != null)
        {
            var shapes = BlockInventoryUI.Instance.GetAllShapes();
            if (shapes.Count > 0)
            {
                int count = Random.Range(1, 3);
                List<(string, int)> rewards = new();

                for (int i = 0; i < count; i++)
                {
                    var randomShape = shapes[Random.Range(0, shapes.Count)];
                    BlockInventory.Instance.AddBlock(randomShape.shapeName, 1);
                    rewards.Add((randomShape.shapeName, 1));
                    Debug.Log($"[DROP] Enemy dropped block: {randomShape.shapeName}");
                }

                BlockInventoryUI.Instance.UpdateAllCounts();

                var popup = FindObjectOfType<BlockRewardPopupUI>();
                if (popup != null)
                    popup.ShowRewards(rewards);
            }
        }

        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    protected bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }
}
