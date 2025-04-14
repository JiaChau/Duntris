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
            buff.Apply(other.GetComponent<PlayerStatsManager>().stats);
            Debug.Log($"Buff picked up: {buff.statType} +{buff.amount}");
            Destroy(gameObject);
        }
    }


}
