using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;
    public string targetTag; // "Enemy" or "Player"

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(targetTag))
        {
            if (targetTag == "Enemy")
            {
                Debug.Log("Projectile hit ENEMY");
                collision.collider.GetComponent<BaseEnemy>()?.TakeDamage(damage);
            }
            else if (targetTag == "Player")
            {
                Debug.Log("Projectile hit PLAYER");
                collision.collider.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log($"Projectile hit {collision.collider.tag}, destroying.");
        }

        Debug.DrawRay(transform.position, collision.contacts[0].normal, Color.red, 1f);
        Destroy(gameObject); // Always destroy after any hit
    }


}
