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
    [Export] TextureButton nextLevelButton;
    [Export] TextureButton mainMenuButton;

    public int levelIndex = -1;
    public bool paused = true;
    const double LOOP_COOLDOWN = 0.1;
    double loopTimer = 0;
    int loopIndex = 0;
    Vector2I playerGridPos;
    public LoopSlot[] instLoopSlots;
    public Direction playerDir = Direction.Up;

    public override void _Ready(){
        playerGridPos = (Vector2I)player.Position.Round() / tileLayer.TileSet.TileSize;
        playerDir = startDirection;
        player.UpdateArrowDirection();

        nextLevelButton.Pressed += () => GameManager.Instance.SwitchToLevel(levelIndex + 1);
        mainMenuButton.Pressed += GameManager.Instance.SwitchToMainMenu;
    }

	public override void _Process(double delta){
		if(paused){
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
        paused = true;
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