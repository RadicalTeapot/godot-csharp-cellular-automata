using Godot;
using System;

public class GUIScene : MarginContainer
{
    [Signal]
    public delegate void HomeClicked();
    [Signal]
    public delegate void PlayPauseClicked(bool playState);
    [Signal]
    public delegate void StepClicked();
    [Signal]
    public delegate void ResetClicked();

    [Export] public bool IsPlaying {
        get { return _isPlaying; }
        set {_isPlaying = value; SetPlayPauseButtonIcon();}
    }

    public override void _Ready()
    {
        _isReady = true;
        SetPlayPauseButtonIcon();
    }

    // Connected in the editor
    private void SetPlayPauseButtonIcon()
    {
        if (_isReady)
        {
            var button = GetNode<Button>("./HBoxContainer/PlayPauseButton");
            if (_isPlaying)
                button.Icon = GD.Load<Texture>("res://assets/pause.png");
            else
                button.Icon = GD.Load<Texture>("res://assets/right.png");
        }
    }

    // Connected in the editor
    public void OnHomeButtonPressed()
    {
        EmitSignal(nameof(HomeClicked));
    }

    // Connected in the editor
    public void OnPlayPauseButtonPressed()
    {
        IsPlaying = !IsPlaying;
        EmitSignal(nameof(PlayPauseClicked), IsPlaying);
    }

    // Connected in the editor
    public void OnStepButtonPressed()
    {
        EmitSignal(nameof(StepClicked));
    }

    // Connected in the editor
    public void OnResetButtonPressed()
    {
        EmitSignal(nameof(ResetClicked));
    }

    private bool _isPlaying;
    private bool _isReady = false;
}
