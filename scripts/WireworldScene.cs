using Godot;

public class WireworldScene : Grid
{
    public override void Reset(int gridWidth, int gridHeight)
    {
        base.Reset(gridWidth, gridHeight);
    }

    public override void UpdateGrid(float delta) {}
    protected override void Clicked(int row, int column) {}
    protected override void Render()
    {
        DrawRect(new Rect2(0, 0, Width * CellSize, Height * CellSize), Color.Color8(255, 255, 255));
        base.Render();
    }
}
