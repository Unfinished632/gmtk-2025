using Godot;
using System;

public partial class SFXButton : TextureButton{
    [Export] AudioStreamPlayer customSFX;

    public override void _Ready(){
        InitSFX();
    }

	protected void InitSFX(){
		if(customSFX != null){
            Pressed += () => customSFX.Play();
            return;
        }

        Pressed += () => GameManager.Instance.ButtonSFX.Play();
	}
}
