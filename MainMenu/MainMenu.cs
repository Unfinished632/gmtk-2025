using Godot;
using System;

public partial class MainMenu : Control{
    [Export] Button playButton;
	[Export] Button exitButton;
	[Export] Button levelSelectionBackButton;

    [Export] Control startMenu;
	[Export] Control levelSelectionMenu;
    [Export] GridContainer levelButtonsContainer;

    [Export] PackedScene LevelButton;

    public override void _Ready(){
        playButton.Pressed += OnPlayButtonPressed;
        exitButton.Pressed += OnExitButtonPressed;

        levelSelectionBackButton.Pressed += SwitchToStartMenu;

        PackedScene[] levels = GameManager.Instance.LevelScenes;

        for(int i = 0; i < levels.Length; i++){
            LevelButton levelButton = LevelButton.Instantiate<LevelButton>();
            levelButton.levelIndex = i;
            levelButtonsContainer.AddChild(levelButton);
        }
    }

	void OnPlayButtonPressed(){
        startMenu.Visible = false;
        levelSelectionMenu.Visible = true;
    }

	void OnExitButtonPressed(){
        GetTree().Quit();
    }

	void SwitchToStartMenu(){
        startMenu.Visible = true;
        levelSelectionMenu.Visible = false;
    }
}
