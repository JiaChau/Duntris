using System.Collections.Generic;
using UnityEngine;

public class BlockInventory : MonoBehaviour
{
    public static BlockInventory Instance { get; private set; }

    private Dictionary<string, int> blockCounts = new Dictionary<string, int>();
    private Dictionary<BlockPieceData, int> inventory = new Dictionary<BlockPieceData, int>();
    private List<BlockPieceData> allAvailablePieces = new List<BlockPieceData>();



    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        blockCounts = new Dictionary<string, int>();
    }

    public void AddBlock(string shapeName, int amount)
    {
        if (!blockCounts.ContainsKey(shapeName))
            blockCounts[shapeName] = 0;

        blockCounts[shapeName] += amount;
        Debug.Log($"[INV] Gained {amount}x {shapeName} (Total: {blockCounts[shapeName]})");
    }

    public bool TryUseBlock(string shapeName)
    {
        if (!blockCounts.ContainsKey(shapeName) || blockCounts[shapeName] <= 0)
        {
            Debug.LogWarning("[INV] Tried to use block with 0 count: " + shapeName);
            return false;
        }

        blockCounts[shapeName]--;
        Debug.Log($"[INV] Used 1x {shapeName} (Remaining: {blockCounts[shapeName]})");
        return true;
    }

    public int GetBlockCount(string shapeName)
    {
        return blockCounts.ContainsKey(shapeName) ? blockCounts[shapeName] : 0;
    }
    public void DecreaseBlock(string name, int amount)
    {
        if (blockCounts.ContainsKey(name))
        {
            blockCounts[name] -= amount;
            if (blockCounts[name] <= 0)
                blockCounts[name] = 0;
        }
    }

    public List<BlockPieceData> GetAllAvailablePieces()
    {
        return new List<BlockPieceData>(inventory.Keys);
    }

    public void AddRandomBlock(int min = 1, int max = 2)
    {
        if (BlockInventoryUI.Instance == null) return;

        var pieces = BlockInventoryUI.Instance.GetAllShapes();
        if (pieces == null || pieces.Count == 0) return;

        int amount = Random.Range(min, max + 1);
        var randomPiece = pieces[Random.Range(0, pieces.Count)];

        AddBlock(randomPiece.shapeName, amount);
        Debug.Log($"[BLOCK] Added {amount}x {randomPiece.shapeName}");
    }




}
