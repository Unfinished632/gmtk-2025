using Godot;
using System;

public partial class CheckboxButton : SFXButton{
	[Export] TextureRect checkTexture;

	[Export] bool isChecked = false;
	public bool IsChecked{
        get{ return isChecked; }
		set{
            isChecked = value;
            UpdateCheck();
        }
    }

	public override void _Ready(){
        InitSFX();
        UpdateCheck();

        Pressed += () => IsChecked = !IsChecked;
    }

	void UpdateCheck(){
		checkTexture.Visible = isChecked;
	}
}
