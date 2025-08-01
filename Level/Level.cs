using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D{
    readonly int DIRECTION_COUNT = Enum.GetValues(typeof(Direction)).Length;

    readonly Vector2I WIN_LEVEL_TILE = new(1, 0);

    [Export] Direction startDirection;
    [Export] Player player;
    [Export] TileMapLayer tileLayer;
    [Export] Control levelWonScreen;
    [Export] Control PauseMenu;
    [Export] TextureButton nextLevelButton;
    [Export] TextureButton winMainMenuButton;
    [Export] TextureButton pauseMainMenuButton;
    [Export] TextureButton pauseResumeButton;

    [Export] AudioStreamPlayer loopIterSFX;

    Vector2 playerStartPos;
    Vector2I playerGridStartPos;

    public int levelIndex = -1;
    public bool loopPaused = true;
    bool paused = false;
    public bool Paused{
        get{ return paused; }
        set{
            paused = value;
            PauseMenu.Visible = paused;
        }
    }
    const double LOOP_COOLDOWN = 0.3;
    double loopTimer = 0;
    int loopIndex = 0;
    Vector2I playerGridPos;
    public LoopSlot[] instLoopSlots;
    public Direction playerDir = Direction.Up;

    public override void _Ready(){
        playerGridPos = (Vector2I)player.Position.Round() / tileLayer.TileSet.TileSize;
        playerDir = startDirection;
        player.UpdateArrowDirection();

        playerStartPos = player.Position;
        playerGridStartPos = playerGridPos;

        nextLevelButton.Pressed += () => GameManager.Instance.SwitchToLevel(levelIndex + 1);
        winMainMenuButton.Pressed += GameManager.Instance.SwitchToMainMenu;
        pauseMainMenuButton.Pressed += GameManager.Instance.SwitchToMainMenu;
        pauseResumeButton.Pressed += () => Paused = false;
    }

	public override void _Process(double delta){
        if(Input.IsActionJustPressed("pause")){
            Paused = !Paused;
        }

		if(loopPaused || Paused){
            return;
        }

        GoOverPlayerLoop(delta);
    }

	void GoOverPlayerLoop(double delta){
		if(loopTimer < LOOP_COOLDOWN){
            loopTimer += delta;
            return;
        }

        loopTimer = 0;

        loopIterSFX.Play();

        int prevLoopIndex = loopIndex - 1 < 0 ? instLoopSlots.Length - 1 : loopIndex - 1;
        instLoopSlots[prevLoopIndex].SetHighlighted(false);
        instLoopSlots[loopIndex].SetHighlighted(true);

        switch(instLoopSlots[loopIndex].selectedInst){
			case PlayerInstruction.Move:
                MovePlayer();
                break;
			case PlayerInstruction.TurnLeft:
                TurnPlayerLeft();
                break;
			case PlayerInstruction.TurnRight:
                TurnPlayerRight();
                break;
        }

        loopIndex++;
        loopIndex = loopIndex >= instLoopSlots.Length ? 0 : loopIndex;

        player.UpdateArrowDirection();
    }

	void MovePlayer(){
        Vector2 tileSize = tileLayer.TileSet.TileSize;

        Vector2 moveDirection = playerDir switch{
            Direction.Up => Vector2.Up,
            Direction.Right => Vector2.Right,
            Direction.Down => Vector2.Down,
            Direction.Left => Vector2.Left,
            _ => Vector2.Zero
        };

        Vector2I movingTileAtlasCoords = tileLayer.GetCellAtlasCoords(playerGridPos + (Vector2I)moveDirection);
        if(movingTileAtlasCoords == new Vector2I(-1, -1)){
            return;
        }

        if(movingTileAtlasCoords == WIN_LEVEL_TILE){
            WinLevel();
        }

        playerGridPos += (Vector2I)moveDirection;
        player.Position += moveDirection * tileSize;
    }

	void TurnPlayerLeft(){
        int enumIndex = (int)playerDir - 1;
        enumIndex = enumIndex < 0 ? DIRECTION_COUNT - 1 : enumIndex;
        playerDir = (Direction)enumIndex;
    }

	void TurnPlayerRight(){
		int enumIndex = (int)playerDir + 1;
		enumIndex = enumIndex > DIRECTION_COUNT - 1 ? 0 : enumIndex;
		playerDir = (Direction)enumIndex;
	}

    void WinLevel(){
        if(levelIndex == GameManager.Instance.LevelScenes.Length - 1){
            GameManager.Instance.SwitchToMainMenu();
            return;
        }

        levelWonScreen.Visible = true;
        loopPaused = true;
    }

    public void Reset(){
        playerDir = startDirection;
        player.Position = playerStartPos;
        playerGridPos = playerGridStartPos;

        int prevLoopIndex = loopIndex - 1 < 0 ? instLoopSlots.Length - 1 : loopIndex - 1;
        instLoopSlots[prevLoopIndex].SetHighlighted(false);

        player.UpdateArrowDirection();
    }
}

public enum PlayerInstruction{
    None,
    Move,
    TurnLeft,
    TurnRight
}

public enum Direction{
	Up,
	Right,
	Down,
	Left
}