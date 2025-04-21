using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockPieceUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text countText;

    private BlockPieceData data;

    public void Init(BlockPieceData pieceData, Sprite icon = null)
    {
        data = pieceData;

        if (icon != null)
        {
            iconImage.sprite = icon;
            iconImage.preserveAspect = true;         // Keeps proportions
            iconImage.rectTransform.sizeDelta = new Vector2(60, 60); // Resize icon
        }

        UpdateCountUI();
    }


    public void UpdateCountUI()
    {
        if (countText != null && BlockInventory.Instance != null)
        {
            int count = BlockInventory.Instance.GetBlockCount(data.shapeName);
            countText.text = count.ToString();
        }
    }

    public void OnClick()
    {
        Debug.Log($"[INVENTORY] Clicked: {data.shapeName}");

        if (BlockPlacementManager.Instance != null)
        {
            BlockPlacementManager.Instance.SelectPiece(data);
        }
        else
        {
            Debug.LogWarning("[INVENTORY] BlockPlacementManager.Instance is null");
        }
    }
}
