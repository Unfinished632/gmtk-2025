using Godot;
using System;

public partial class LoopSlot : Control{
	readonly int PLAYER_INST_COUNT = Enum.GetValues(typeof(PlayerInstruction)).Length;

    public Level level;

    [Export] Label instLabel;
    public PlayerInstruction selectedInst = PlayerInstruction.None;

    public override void _Ready(){
        UpdateDisplay();

        GuiInput += OnGuiInput;
    }

	void OnGuiInput(InputEvent inputEvent){
		if(!level.paused){
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
        instLabel.Text = Enum.GetName(typeof(PlayerInstruction), selectedInst);
    }
}
