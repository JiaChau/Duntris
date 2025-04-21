using UnityEngine;
using TMPro;

public class BlockGrid : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;
    public TMP_Text scoreText;

    private GridTile[,] tiles = new GridTile[10, 10];
    private int sessionScore = 0;
    public static int cumulativeScore = 0;


    void Start()
    {
        sessionScore = 0;
        GenerateGrid();
        UpdateScoreUI();
    }

    void GenerateGrid()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                GameObject tileObj = Instantiate(tilePrefab, gridParent);
                GridTile tile = tileObj.GetComponent<GridTile>();
                tile.gridPos = new Vector2Int(x, y);
                tile.tileImage = tileObj.GetComponent<UnityEngine.UI.Image>();
                tile.SetOccupied(false);

                tiles[x, y] = tile;
            }
        }
    }

    public bool CanPlaceBlockAt(BlockPieceData piece, Vector2Int anchorPos)
    {
        bool[,] shape = piece.GetShape2D();
        int height = shape.GetLength(0);
        int width = shape.GetLength(1);

        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                if (!shape[r, c]) continue;

                int x = anchorPos.x + (c - piece.anchor.x);
                int y = anchorPos.y + (r - piece.anchor.y);

                if (x < 0 || y < 0 || x >= 10 || y >= 10 || tiles[x, y].isOccupied)
                    return false;
            }
        }

        return true;
    }

    public void PlaceBlock(BlockPieceData piece, Vector2Int anchorPos)
    {
        bool[,] shape = piece.GetShape2D();
        int height = shape.GetLength(0);
        int width = shape.GetLength(1);

        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                if (!shape[r, c]) continue;

                int x = anchorPos.x + (c - piece.anchor.x);
                int y = anchorPos.y + (r - piece.anchor.y);
                tiles[x, y].SetOccupied(true);
            }
        }

        BlockInventoryUI.Instance.UpdateAllCounts();
        BlockPlacementManager.Instance.ClearSelected();

        CheckAndClearLines();
        UpdateScoreUI();
    }

    void CheckAndClearLines()
    {
        int linesCleared = 0;

        // Rows
        for (int y = 0; y < 10; y++)
        {
            bool complete = true;
            for (int x = 0; x < 10; x++)
            {
                if (!tiles[x, y].isOccupied)
                {
                    complete = false;
                    break;
                }
            }
            if (complete)
            {
                for (int x = 0; x < 10; x++)
                    tiles[x, y].SetOccupied(false);
                linesCleared++;
                Debug.Log($"[CLEAR] Row {y} cleared.");
            }
        }

        // Columns
        for (int x = 0; x < 10; x++)
        {
            bool complete = true;
            for (int y = 0; y < 10; y++)
            {
                if (!tiles[x, y].isOccupied)
                {
                    complete = false;
                    break;
                }
            }
            if (complete)
            {
                for (int y = 0; y < 10; y++)
                    tiles[x, y].SetOccupied(false);
                linesCleared++;
                Debug.Log($"[CLEAR] Column {x} cleared.");
            }
        }

        if (linesCleared > 0)
        {
            int basePoints = 10 * linesCleared;
            int bonus = (linesCleared >= 2) ? 25 * (linesCleared - 1) : 0;
            int finalScore = (basePoints * linesCleared) + bonus;

            sessionScore += finalScore;
            cumulativeScore += finalScore;


            Debug.Log($"[SCORE] +{finalScore} points (x{linesCleared})");

        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + sessionScore;
    }

    public void ClearGrid()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (tiles[x, y] != null)
                    tiles[x, y].SetOccupied(false);
            }
        }

        sessionScore = 0;
        UpdateScoreUI();
    }

    public static int GetCumulativeScore()
    {
        return cumulativeScore;
    }


}
