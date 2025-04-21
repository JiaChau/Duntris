using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridTile : MonoBehaviour, IPointerClickHandler
{
    public Image tileImage;
    public Vector2Int gridPos;
    public bool isOccupied = false;

    public void SetOccupied(bool value)
    {
        isOccupied = value;
        tileImage.color = value ? Color.green : Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var selected = BlockPlacementManager.Instance.selectedBlock;
        if (selected == null) return;

        var grid = FindObjectOfType<BlockGrid>();

        // First: Check if this placement is allowed
        if (!grid.CanPlaceBlockAt(selected, gridPos))
        {
            Debug.Log("[GRID] Invalid placement: tile occupied or out of bounds.");
            return;
        }

        // Then: Check inventory before placing
        if (!BlockInventory.Instance.TryUseBlock(selected.shapeName))
        {
            Debug.LogWarning("[GRID] Tried to place a block you don't own.");
            return;
        }

        // All checks passed, place block
        grid.PlaceBlock(selected, gridPos);
        BlockInventoryUI.Instance.UpdateAllCounts();
        BlockPlacementManager.Instance.ClearSelected();
    }
}
