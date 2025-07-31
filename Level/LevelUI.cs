using Godot;
using System;

public partial class LevelUI : Control{
    [Export] Level level;
    [Export] PackedScene loopSlotScene;
    [Export] HBoxContainer loopSlotsContainer;
    [Export] TextureButton startButton;
    [Export(PropertyHint.Range, "1, 10")] int loopSlotsCount = 1;

    LoopSlot[] loopSlots = [];

    public override void _Ready(){
        loopSlots = new LoopSlot[loopSlotsCount];
        for(int i = 0; i < loopSlotsCount; i++){
            loopSlots[i] = loopSlotScene.Instantiate<LoopSlot>();
            loopSlots[i].level = level;
            loopSlotsContainer.AddChild(loopSlots[i]);
        }
        level.instLoopSlots = loopSlots;

        startButton.Pressed += OnStartButtonPressed;
    }

	void OnStartButtonPressed(){
        level.loopPaused = false;
    }
}
