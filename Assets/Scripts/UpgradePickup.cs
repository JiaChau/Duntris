using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    public StatBuffLibrary buffLibrary;
    private StatBuff buff;

    void Start()
    {
        if (buffLibrary != null)
        {
            buff = buffLibrary.GetRandomBuff();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && buff != null)
        {
            var playerStats = other.GetComponent<PlayerStatsManager>().stats;

            // Apply the stat buff
            buff.Apply(playerStats);

            Debug.Log($"Buff picked up: {buff.statType} +{buff.amount}");

            // Trigger UI update if BuffStatsUI exists
            BuffStatsUI ui = FindObjectOfType<BuffStatsUI>();
            if (ui != null)
            {
                ui.UpdateStats(playerStats);
                ui.FlashBuff("+" + buff.amount + " " + buff.statType.ToString());
            }

            Destroy(gameObject);
        }
    }
}
