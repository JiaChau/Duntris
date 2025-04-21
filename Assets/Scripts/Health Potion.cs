using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int minHealAmount = 10;
    public int maxHealAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        int healAmount = Random.Range(minHealAmount, maxHealAmount + 1);

        PlayerHealth.Instance.Heal(healAmount);

        // Show healing popup if UI exists
        var popup = FindObjectOfType<HealthGainPopupUI>();
        if (popup != null)
        {
            popup.ShowHealAmount(healAmount);
        }

        Destroy(gameObject);
    }
}
