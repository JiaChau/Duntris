using UnityEngine;
using System.Collections.Generic;

public class TreasureChest : MonoBehaviour
{
    public int minBlocks = 1;
    public int maxBlocks = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (BlockInventoryUI.Instance != null && BlockInventory.Instance != null)
        {
            var shapes = BlockInventoryUI.Instance.GetAllShapes();
            if (shapes.Count > 0)
            {
                int count = Random.Range(minBlocks, maxBlocks + 1);
                List<(string, int)> rewards = new();

                for (int i = 0; i < count; i++)
                {
                    var randomShape = shapes[Random.Range(0, shapes.Count)];
                    BlockInventory.Instance.AddBlock(randomShape.shapeName, 1);
                    rewards.Add((randomShape.shapeName, 1));
                    Debug.Log($"[CHEST] Gave block: {randomShape.shapeName}");
                }

                BlockInventoryUI.Instance.UpdateAllCounts();

                var popup = FindObjectOfType<BlockRewardPopupUI>();
                if (popup != null)
                    popup.ShowRewards(rewards);
            }
        }

        Destroy(gameObject); // Remove the chest after collection
    }
}
