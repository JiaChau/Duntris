using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public float healAmount = 20f;
    public bool increasesMaxHealth = false;

    // Update this method
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<PlayerHealth>();
            health.Heal(healAmount, increasesMaxHealth);

            string healType = increasesMaxHealth ? "Max HP & Current HP" : "Current HP";
            Debug.Log($"Potion picked up: Healed {healAmount} ({healType})");

            Destroy(gameObject);
        }
    }

}
