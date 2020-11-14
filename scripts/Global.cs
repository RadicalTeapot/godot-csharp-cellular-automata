using Godot;

public class Global : Node
{
    public Node CurrentScene;
    public override void _Ready()
    {
        var root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
    }

    public void GoToScene(string path)
    {
        CallDeferred(nameof(DeferredGoToScene), path);
    }

    public void DeferredGoToScene(string path)
    {
        CurrentScene.Free();
        CurrentScene = GD.Load<PackedScene>(path).Instance();
        GetTree().Root.AddChild(CurrentScene);
        GetTree().CurrentScene = CurrentScene;
    }

    public void GoToMainScene(PackedScene GameMode)
    {
        CallDeferred(nameof(DeferredGoToMainScene), GameMode);
    }

    public void DeferredGoToMainScene(PackedScene GameMode)
    {
        DeferredGoToScene("res://scenes/MainScene.tscn");
        ((MainScene)CurrentScene).CurrentPackedScene = GameMode;
    }
}
