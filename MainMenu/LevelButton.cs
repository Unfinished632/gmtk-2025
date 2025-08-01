using Godot;
using System;

public partial class LevelButton : SFXButton{
    [Export] Label label;

    public int levelIndex = -1;

    public override void _Ready(){
        InitSFX();

        Pressed += OnPressed;
        label.Text = (levelIndex + 1).ToString();
    }

	void OnPressed(){
        GameManager.Instance.SwitchToLevel(levelIndex);
    }
}
