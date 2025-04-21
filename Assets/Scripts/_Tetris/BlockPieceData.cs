using UnityEngine;

[System.Serializable]
public class BlockPieceData
{
    public string shapeName;
    public int width;
    public int height;
    public int[] flatShape;
    public Vector2Int anchor;
    public Sprite icon; // must be assigned in code or inspector

    public BlockPieceData(string name, int w, int h, int[] shape, Vector2Int anchor, Sprite icon = null)
    {
        shapeName = name;
        width = w;
        height = h;
        flatShape = shape;
        this.anchor = anchor;
        this.icon = icon;
    }

    public bool[,] GetShape2D()
    {
        bool[,] shape = new bool[height, width];
        for (int i = 0; i < flatShape.Length; i++)
        {
            int row = i / width;
            int col = i % width;
            shape[row, col] = flatShape[i] != 0;
        }
        return shape;
    }
}
