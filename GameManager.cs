using Godot;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public partial class GameManager : Node{
	public static GameManager Instance{ get; private set; }

    const string LEVELS_DIR = "res://Level/Levels";

    [Export] PackedScene mainMenuScene;
    public PackedScene[] LevelScenes{ get; private set; }

    public MainMenu MainMenu{ get; private set; }
    public Level CurrentLevel{ get; private set; }

    [Export] public AudioStreamPlayer ButtonSFX{ get; private set; }
    [Export] public AudioStreamPlayer LoopSlotSFX{ get; private set; }
    [Export] public AudioStreamPlayer LevelWinSFX{ get; private set; }

    public bool isFullscreen = false;

    public override void _Ready(){
		if(Instance != null){
            QueueFree();
            GD.PushWarning("Destroyed duplicate GameManager");
            return;
        }

        Instance = this;

        string[] levelFiles = ResourceLoader.ListDirectory(LEVELS_DIR);
        levelFiles = [.. levelFiles.OrderBy(x => int.Parse(IntegerRegex().Match(x).Value))];

        LevelScenes = new PackedScene[levelFiles.Length];

        for(int i = 0; i < levelFiles.Length; i++){
            LevelScenes[i] = GD.Load<PackedScene>(LEVELS_DIR + '/' + levelFiles[i]);
        }

        MainMenu = mainMenuScene.Instantiate<MainMenu>();
        AddChild(MainMenu);
    }

	public void SwitchToLevel(int levelIndex){
        MainMenu?.QueueFree();
        MainMenu = null;
        CurrentLevel?.QueueFree();
        CurrentLevel = LevelScenes[levelIndex].Instantiate<Level>();
        CurrentLevel.levelIndex = levelIndex;
        AddChild(CurrentLevel);
    }

	public void SwitchToMainMenu(){
        CurrentLevel.QueueFree();
        CurrentLevel = null;
        MainMenu = mainMenuScene.Instantiate<MainMenu>();
        AddChild(MainMenu);
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex IntegerRegex();
}
