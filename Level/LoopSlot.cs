using Godot;
using System;

public partial class LoopSlot : Control{
	readonly int PLAYER_INST_COUNT = Enum.GetValues(typeof(PlayerInstruction)).Length;

    public Level level;

    [Export] CompressedTexture2D moveTexture;
    [Export] CompressedTexture2D turnLeftTexture;
    [Export] CompressedTexture2D turnRightTexture;
    [Export] TextureRect instTexture;
    [Export] Label instLabel;
    public PlayerInstruction selectedInst = PlayerInstruction.None;

    public override void _Ready(){
        UpdateDisplay();

        GuiInput += OnGuiInput;
    }

	void OnGuiInput(InputEvent inputEvent){
		if(!level.loopPaused){
            return;
        }

		if(inputEvent.GetType() != typeof(InputEventMouseButton)){
            return;
        }

        var mouseEvent = (InputEventMouseButton)inputEvent;

		if(!mouseEvent.Pressed || mouseEvent.ButtonIndex != MouseButton.Left){
            return;
        }

		int enumIndex = (int)selectedInst + 1;
		enumIndex = enumIndex > PLAYER_INST_COUNT - 1 ? 0 : enumIndex;
		selectedInst = (PlayerInstruction)enumIndex;

        UpdateDisplay();
    }

	void UpdateDisplay(){
        switch(selectedInst){
            case PlayerInstruction.None:
                instLabel.Text = "None";
                instTexture.Texture = null;
                break;
            case PlayerInstruction.Move:
                instLabel.Text = "Move";
                instTexture.Texture = moveTexture;
                break;
            case PlayerInstruction.TurnLeft:
                instLabel.Text = "Turn Left";
                instTexture.Texture = turnLeftTexture;
                break;
            case PlayerInstruction.TurnRight:
                instLabel.Text = "Turn Right";
                instTexture.Texture = turnRightTexture;
                break;
        }
    }
}
