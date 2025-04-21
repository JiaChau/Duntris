using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class BlockInventoryUI : MonoBehaviour
{
    public Transform inventoryHolder;
    public GameObject blockButtonPrefab;

    public static BlockInventoryUI Instance { get; private set; }

    private Dictionary<string, BlockPieceUI> uiButtons = new();

    // Hardcoded block shapes
    private List<BlockPieceData> shapes = new List<BlockPieceData>();
    private Sprite[] iconSprites; // From sliced sprite sheet

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Load all sliced icons from Resources folder
        iconSprites = Resources.LoadAll<Sprite>("BlockIcons/Block Icons");
    }

    void Start()
    {
        GenerateDefaultShapes();
        // GiveStarterInventory();
        Populate();
    }

    void GenerateDefaultShapes()
    {
        shapes.Clear();

        shapes.Add(new BlockPieceData("Square", 2, 2, new int[] {
            1, 1,
            1, 1
        }, new Vector2Int(0, 0)));

        shapes.Add(new BlockPieceData("Line", 1, 4, new int[] {
            1,
            1,
            1,
            1
        }, new Vector2Int(0, 0)));

        shapes.Add(new BlockPieceData("L", 2, 3, new int[] {
            1, 0,
            1, 0,
            1, 1
        }, new Vector2Int(0, 0)));

        shapes.Add(new BlockPieceData("T", 3, 2, new int[] {
            1, 1, 1,
            0, 1, 0
        }, new Vector2Int(1, 0)));

        shapes.Add(new BlockPieceData("Z", 3, 2, new int[] {
            1, 1, 0,
            0, 1, 1
        }, new Vector2Int(1, 0)));

        shapes.Add(new BlockPieceData("Plus", 3, 3, new int[] {
            0, 1, 0,
            1, 1, 1,
            0, 1, 0
        }, new Vector2Int(1, 1)));
    }

    void GiveStarterInventory()
    {
        foreach (var piece in shapes)
        {
            BlockInventory.Instance.AddBlock(piece.shapeName, 3);
        }
    }

    void Populate()
    {
        foreach (Transform child in inventoryHolder)
            Destroy(child.gameObject);

        uiButtons.Clear();

        foreach (var piece in shapes)
        {
            GameObject btn = Instantiate(blockButtonPrefab, inventoryHolder);
            BlockPieceUI ui = btn.GetComponent<BlockPieceUI>();

            // Load sprite based on shapeName from Resources/BlockIcons/
            Sprite icon = Resources.Load<Sprite>("BlockIcons/" + piece.shapeName);
            if (icon == null)
            {
                Debug.LogWarning("[ICON] Missing icon for: " + piece.shapeName);
            }

            ui.Init(piece, icon);  // Pass the loaded icon
            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ui.OnClick);

            if (!uiButtons.ContainsKey(piece.shapeName))
                uiButtons[piece.shapeName] = ui;
        }
    }



    Sprite LoadIcon(string name)
    {
        switch (name)
        {
            case "Square": return iconSprites.Length > 0 ? iconSprites[0] : null;
            case "L": return iconSprites.Length > 1 ? iconSprites[1] : null;
            case "Line": return iconSprites.Length > 2 ? iconSprites[2] : null;
            case "Z": return iconSprites.Length > 3 ? iconSprites[3] : null;
            case "T": return iconSprites.Length > 4 ? iconSprites[4] : null;
            case "Plus": return iconSprites.Length > 5 ? iconSprites[5] : null;
            default:
                Debug.LogWarning("[ICON] No icon found for: " + name);
                return null;
        }
    }

    public void UpdateAllCounts()
    {
        foreach (var entry in uiButtons)
        {
            entry.Value.UpdateCountUI();
        }
    }

    public List<BlockPieceData> GetAllShapes()
    {
        return shapes;
    }


}