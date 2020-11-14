using Godot;

public class SandScene : Grid
{
    private class CellType
    {
        public const byte AIR = 0;
        public const byte SAND = 1;
        public const byte WATER = 2;
        public const byte WOOD = 3;
    }

    public override void Reset(int gridWidth, int gridHeight)
    {
        base.Reset(gridWidth, gridHeight);
        for (int i = 0; i < _count; i++)
            _cells[i] = new Cell { index = i, type = CellType.AIR };

        // Testing purposes
        _cells[10] = new Cell { index = 10, type = CellType.WATER };
        _cells[20] = new Cell { index = 20, type = CellType.WATER };
        _cells[30] = new Cell { index = 30, type = CellType.WATER };
    }

    public override void UpdateGrid(float delta)
    {
        // It would probably be better to go from bottom up than top to bottom
        for (int i = 0; i < _count; i++)
        {
            int nextCell = -1;
            switch (_cells[i].type)
            {
                case CellType.WATER:
                    if (_neighbourIndices[i].Bottom != CellNeighbourhoodIndices.INVALID && _cells[_neighbourIndices[i].Bottom].type == CellType.AIR)
                        nextCell = _neighbourIndices[i].Bottom;
                    else if (_neighbourIndices[i].Left != CellNeighbourhoodIndices.INVALID && _cells[_neighbourIndices[i].Left].type == CellType.AIR)
                        nextCell = _neighbourIndices[i].Left;
                    else if (_neighbourIndices[i].Right != CellNeighbourhoodIndices.INVALID && _cells[_neighbourIndices[i].Right].type == CellType.AIR)
                        nextCell = _neighbourIndices[i].Right;
                    break;
                default:
                    break;
            }
            if (nextCell > -1)
            {
                _cells[nextCell].nextType = _cells[i].type;
                _cells[i].nextType = CellType.AIR;
            }
        }
        for(int i = 0; i < _count; i++)
        {
            _cells[i].type = _cells[i].nextType;
            _cells[i].nextType = CellType.AIR;
        }
    }

    protected override void Render()
    {
        int x = 0, y = 0;
        for (int i = 0; i < _count; i++)
        {
            x = i % Width;
            y = i / Width;
            switch (_cells[i].type)
            {
                case CellType.WATER:
                    DrawRect(new Rect2(x * CellSize, y * CellSize, CellSize, CellSize), Color.Color8(100, 100, 255));
                    break;
                default:
                    break;
            }
        }
        base.Render();
    }
}
