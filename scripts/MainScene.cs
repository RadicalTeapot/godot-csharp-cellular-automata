using System;
using Godot;

public class MainScene : Control
{
    public static readonly PackedScene GameOfLife = GD.Load<PackedScene>("res://scenes/GameOfLifeScene.tscn");
    public static readonly PackedScene Sand = GD.Load<PackedScene>("res://scenes/SandScene.tscn");
    public static readonly PackedScene Wireworld = GD.Load<PackedScene>("res://scenes/WireworldScene.tscn");
    [Export] public PackedScene CurrentPackedScene {
        get { return _CurrentPackedScene; }
        set { CallDeferred(nameof(DeferredSetCurrentScene), value); }
    }
    [Export] public int CellSize
    {
        get { return _CellSize; }
        set
        {
            _CellSize = value;
            HandleReset();
        }
    }
    [Export] public bool DrawGrid
    {
        get { return _DrawGrid; }
        set
        {
            _DrawGrid = value;
            if (_CurrentScene != null)
                _CurrentScene.DrawGrid = _DrawGrid;
        }
    }
    public GUIScene GUI { get { return _gui; } }

    public override void _Ready()
    {
        _gui = GetNode<GUIScene>("VBoxContainer/UI");
        _gui.Connect(nameof(GUIScene.StepClicked), this, nameof(HandleStep));
        _gui.Connect(nameof(GUIScene.PlayPauseClicked), this, nameof(HandlePlayPause));
        _gui.Connect(nameof(GUIScene.ResetClicked), this, nameof(HandleReset));
        _gui.Connect(nameof(GUIScene.HomeClicked), this, nameof(HandleHome));
    }

    public void DeferredSetCurrentScene(PackedScene packedScene)
    {
        var instance = packedScene.Instance();
        if (!(instance is Grid))
            throw new ArgumentException("Invalid main scene mode");

        if (_CurrentScene != null)
            _CurrentScene.Free();
        _CurrentPackedScene = packedScene;
        _CurrentScene = _CurrentPackedScene.Instance() as Grid;

        // Setup scene
        _CurrentScene.IsPlaying = _gui.IsPlaying;
        _CurrentScene.DrawGrid = DrawGrid;
        HandleReset();

        // Add scene to tree
        GetNode<Control>("VBoxContainer/GridContainer").AddChild(_CurrentScene);
    }

    public void HandleStep()
    {
        if (_CurrentScene != null)
            _CurrentScene.UpdateGrid(0);
    }

    public void HandleReset()
    {
        if (_CurrentScene != null)
        {
            var sceneContainer = GetNode<Control>("VBoxContainer/GridContainer");
            int width = (int)Math.Ceiling(sceneContainer.RectSize.x / _CellSize);
            int height = (int)Math.Ceiling(sceneContainer.RectSize.y / _CellSize);
            _CurrentScene.Reset(width, height);
            _CurrentScene.CellSize = _CellSize;
        }
    }

    public void HandlePlayPause(bool playState)
    {
        if (_CurrentScene != null)
            _CurrentScene.IsPlaying = playState;
    }

    public void HandleHome()
    {
        GetNode<Global>("/root/Global").GoToScene("res://scenes/StartScreen.tscn");
    }

    private GUIScene _gui = null;
    private Grid _CurrentScene = null;
    private PackedScene _CurrentPackedScene;
    private int _CellSize = 10;
    private bool _DrawGrid = false;
}
