using Godot;

public class StartScreen : Control
{
    public void OnStartGameOfLife()
    {
        GetNode<Global>("/root/Global").GoToMainScene(MainScene.GameOfLife);
    }
    public void OnStartWireworld()
    {
        GetNode<Global>("/root/Global").GoToMainScene(MainScene.Wireworld);
    }
    public void OnStartSand()
    {
        GetNode<Global>("/root/Global").GoToMainScene(MainScene.Sand);
    }
}
