using System;
using System.Threading.Tasks;
using Godot;

public class GameOfLifeScene : Grid
{
    private class CellType
    {
        public const byte OFF = 0;
        public const byte ON = 1;
    }

    public override void Reset(int gridWidth, int gridHeight)
    {
        base.Reset(gridWidth, gridHeight);
        for (int i = 0; i < _count; i++)
            _cells[i] = new Cell { index = i, type = CellType.OFF };
    }

    protected override void Render()
    {
        DrawRect(new Rect2(0, 0, Width * CellSize, Height * CellSize), Color.Color8(255, 255, 255));
        int x = 0, y = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_cells[i].type == CellType.ON)
            {
                x = i % Width;
                y = i / Width;
                DrawRect(new Rect2(x * CellSize, y * CellSize, CellSize, CellSize), Color.Color8(0, 0, 0));
            }
        }
        base.Render();
    }

    protected override void Clicked(int row, int column)
    {
        int index = row + column * Width;
        if (index >= 0 && index < _count && index != _lastClickedIndex)
        {
            _cells[index].type = (byte)(1 - _cells[index].type);
            _lastClickedIndex = index;
        }
    }

    public override void UpdateGrid(float delta)
    {
        Parallel.For(0, _count, i =>
        {
            int x = i % Width;
            int y = i / Width;
            int sum = GetNeighbourhoodSum(i);
            if (_cells[i].type == CellType.ON)
            {
                if (sum < 2 || sum > 3)
                    _cells[i].nextType = CellType.OFF;
                else
                    _cells[i].nextType = CellType.ON;
            }
            else
            {
                if (sum == 3)
                    _cells[i].nextType = CellType.ON;
            }
        });
        for (int i = 0; i < _count; i++)
            _cells[i].type = _cells[i].nextType;
    }

    private int GetNeighbourhoodSum(int index)
    {
        // In the current case there's no invalid id so using the index without checking its validity is fine
        return _cells[_neighbourIndices[index].TopLeft].type +
            _cells[_neighbourIndices[index].Top].type +
            _cells[_neighbourIndices[index].TopRight].type +
            _cells[_neighbourIndices[index].Left].type +
            _cells[_neighbourIndices[index].Right].type +
            _cells[_neighbourIndices[index].BottomLeft].type +
            _cells[_neighbourIndices[index].Bottom].type +
            _cells[_neighbourIndices[index].BottomRight].type;
    }

    private int _lastClickedIndex = -1;
}
