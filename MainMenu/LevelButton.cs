using Godot;
using System;

public partial class LevelButton : Button{
    public int levelIndex = 10;

    public override void _Ready(){
        Pressed += OnPressed;
        Text = (levelIndex + 1).ToString();
    }

	void OnPressed(){
        GameManager.Instance.SwitchToLevel(levelIndex);
    }
}
