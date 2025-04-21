using UnityEngine;

public class BlockPlacementManager : MonoBehaviour
{
    public static BlockPlacementManager Instance { get; private set; }

    public BlockPieceData selectedBlock;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void SelectPiece(BlockPieceData piece)
    {
        if (BlockInventory.Instance.GetBlockCount(piece.shapeName) > 0)
        {
            selectedBlock = piece;
            Debug.Log("[BLOCK] Selected: " + piece.shapeName);
        }
        else
        {
            Debug.Log("[BLOCK] No blocks left for: " + piece.shapeName);
        }
    }



    public void ClearSelected()
    {
        selectedBlock = null;
    }
    
}
