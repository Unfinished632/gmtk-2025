using Godot;
using System;

public partial class GameManager : Node{
	public static GameManager Instance{ get; private set; }

    const string LEVELS_DIR = "res://Level/Levels";

    [Export] PackedScene mainMenuScene;
    public PackedScene[] LevelScenes{ get; private set; }

    public MainMenu MainMenu{ get; private set; }
    public Level CurrentLevel{ get; private set; }

    public override void _Ready(){
		if(Instance != null){
            QueueFree();
            GD.PushWarning("Destroyed duplicate GameManager");
            return;
        }

        Instance = this;

        DirAccess dir = DirAccess.Open(LEVELS_DIR);
        string[] levelFiles = dir.GetFiles();

        LevelScenes = new PackedScene[levelFiles.Length];

        for(int i = 0; i < levelFiles.Length; i++){
            LevelScenes[i] = GD.Load<PackedScene>(LEVELS_DIR + '/' + levelFiles[i]);
        }

        MainMenu = mainMenuScene.Instantiate<MainMenu>();
        AddChild(MainMenu);
    }

	public void SwitchToLevel(int levelIndex){
        MainMenu.QueueFree();
        CurrentLevel = LevelScenes[levelIndex].Instantiate<Level>();
        AddChild(CurrentLevel);
    }

	public void SwitchToMainMenu(){
        CurrentLevel.QueueFree();
        MainMenu = mainMenuScene.Instantiate<MainMenu>();
        AddChild(MainMenu);
    }
}
