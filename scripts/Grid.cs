using System;
using Godot;

public struct Cell
{
    // Max square grid side length is then sqrt(int.MaxValue)~=46340 with is ~80gb of memory
    // for the current data scheme (Cell is 41 bits) so we're safe using an int here
    public int index;
    public byte type;
    public byte nextType;
}
public class CellNeighbourhoodIndices
{
    public readonly static int INVALID = -1;
    public int TopLeft = CellNeighbourhoodIndices.INVALID;
    public int Top = CellNeighbourhoodIndices.INVALID;
    public int TopRight = CellNeighbourhoodIndices.INVALID;
    public int Right = CellNeighbourhoodIndices.INVALID;
    public int Current = CellNeighbourhoodIndices.INVALID;
    public int Left = CellNeighbourhoodIndices.INVALID;
    public int BottomLeft = CellNeighbourhoodIndices.INVALID;
    public int Bottom = CellNeighbourhoodIndices.INVALID;
    public int BottomRight = CellNeighbourhoodIndices.INVALID;
    public CellNeighbourhoodIndices(int[] indices)
    {
        TopLeft = indices[0];
        Top = indices[1];
        TopRight = indices[2];
        Left = indices[3];
        Current = indices[4];
        Right = indices[5];
        BottomLeft = indices[6];
        Bottom = indices[7];
        BottomRight = indices[8];
    }
}

public class Grid: Node2D
{
    [Export] public bool UseDiagonalNeighbours = false;
    [Export] public bool AreNeighboursWrapped = false;
    [Export] public int Width = 100;
    [Export] public int Height = 100;
    [Export] public int CellSize = 10;
    [Export] public bool DrawGrid = false;
    [Export] public bool IsPlaying = true;

    public override void _Ready()
    {
        Reset(Width, Height);
    }

    virtual public void Reset(int gridWidth, int gridHeight)
    {
        Width = gridWidth;
        Height = gridHeight;
        _count = gridWidth * gridHeight;
        _cells = new Cell[_count];
        _neighbourIndices = new CellNeighbourhoodIndices[_count];
        BuildNeighbourIndices();
    }
    virtual public void UpdateGrid(float delta) {}
    virtual protected void Clicked(int row, int column) {}
    virtual protected void Render()
    {
        if (DrawGrid)
        {
            for (int i = 0; i <= Width; i++)
                DrawLine(new Vector2(i * CellSize, 0), new Vector2(i * CellSize, Height * CellSize), Color.Color8(0, 0, 0, 25));
            for (int i = 0; i <= Height; i++)
                DrawLine(new Vector2(0, i * CellSize), new Vector2(Width * CellSize, i * CellSize), Color.Color8(0, 0, 0, 25));
        }
    }

    private void BuildNeighbourIndices()
    {
        for (int i = 0; i < _count; i++)
        {
            int x = i % Width;
            int y = i / Width;
            int[] indices = new int[9];
            for (int s = -1; s <= 1; s++)
                for (int t = -1; t <= 1; t++)
                {
                    int value;
                    // Skip diagonals
                    if (!UseDiagonalNeighbours && Math.Abs(s) + Math.Abs(t) == 2)
                    {
                        value = CellNeighbourhoodIndices.INVALID;
                    }
                    else
                    {
                        int wrappedX = x + s;
                        int wrappedY = y + t;
                        if (!AreNeighboursWrapped && (wrappedX < 0 || wrappedY < 0 || wrappedX >= Width || wrappedY >= Height))
                        {
                            value = CellNeighbourhoodIndices.INVALID;
                        }
                        else
                        {
                            if (AreNeighboursWrapped)
                            {
                                if (wrappedX < 0)
                                    wrappedX += Width;
                                else if (wrappedX >= Width)
                                    wrappedX -= Width;
                                if (wrappedY < 0)
                                    wrappedY += Height;
                                else if (wrappedY >= Height)
                                    wrappedY -= Height;
                            }
                            value = wrappedX + wrappedY * Width;
                        }
                    }
                    indices[(s + 1) + (t + 1) * 3] = value;
                }
            _neighbourIndices[i] = new CellNeighbourhoodIndices(indices);
        }
    }


    public override void _Process(float delta)
    {
        Update();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (IsPlaying)
           UpdateGrid(delta);
    }


    public override void _Draw()
    {
        Render();
    }
    public override void _Input(InputEvent @event)
    {
        Vector2 position = new Vector2();
        if (@event is InputEventMouseButton mouseEvent)
        {
           if (mouseEvent.Pressed && (ButtonList)mouseEvent.ButtonIndex == ButtonList.Left)
                _mouseDown = true;
            else if (!mouseEvent.Pressed)
                _mouseDown = false;
            position = mouseEvent.Position;
        }
        else if (@event is InputEventMouseMotion mouseMotion) {
            position = mouseMotion.Position;
        }

        if (_mouseDown)
            Clicked((int)Math.Floor(position.x / CellSize), (int)Math.Floor(position.y / CellSize));
    }


    protected Cell[] _cells;
    protected CellNeighbourhoodIndices[] _neighbourIndices;
    protected int _count;
    private bool _mouseDown;
}
