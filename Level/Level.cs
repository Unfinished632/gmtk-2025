using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D{
    readonly int DIRECTION_COUNT = Enum.GetValues(typeof(Direction)).Length;

    [Export] Player player;
    [Export] TileMapLayer tileLayer;

    public bool paused = true;
    const double LOOP_COOLDOWN = 0.1;
    double loopTimer = 0;
    int loopIndex = 0;
    Vector2I playerGridPos;
    public LoopSlot[] instLoopSlots;
    public Direction playerDir = Direction.Up;

    public override void _Ready(){
        playerGridPos = (Vector2I)player.Position.Round() / tileLayer.TileSet.TileSize;
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
        Vector2 playerPos = player.Position;

        switch(playerDir){
			case Direction.Up:
                playerGridPos.Y--;
                playerPos.Y -= tileSize.Y;
                break;
			case Direction.Right:
				playerGridPos.X++;
				playerPos.X += tileSize.X;
                break;
			case Direction.Down:
				playerGridPos.Y++;
				playerPos.Y += tileSize.Y;
                break;
			case Direction.Left:
				playerGridPos.X--;
				playerPos.X -= tileSize.X;
                break;
        }

        player.Position = playerPos;
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