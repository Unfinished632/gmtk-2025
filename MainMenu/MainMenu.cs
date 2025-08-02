using Godot;
using System;
using static Godot.DisplayServer;

public partial class MainMenu : Control{
    [Export] TextureButton playButton;
	[Export] TextureButton exitButton;
	[Export] TextureButton levelSelectionBackButton;

    [Export] Control startMenu;
	[Export] Control levelSelectionMenu;
    [Export] GridContainer levelButtonsContainer;

    [Export] CheckboxButton fullscreenCheckbox;

    [Export] PackedScene LevelButton;

    public override void _Ready(){
        playButton.Pressed += OnPlayButtonPressed;
        exitButton.Pressed += OnExitButtonPressed;

        levelSelectionBackButton.Pressed += SwitchToStartMenu;

        fullscreenCheckbox.Pressed += OnFullscreenCheckboxPressed;

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

    void OnFullscreenCheckboxPressed(){
        DisplayServer.WindowSetMode(fullscreenCheckbox.IsChecked ? WindowMode.Fullscreen : WindowMode.Windowed);
    }
}
