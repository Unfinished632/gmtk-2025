using Godot;
using System;

public partial class Player : Node2D{
    [Export] Level level;
    [Export] Sprite2D upArrow;
	[Export] Sprite2D rightArrow;
	[Export] Sprite2D downArrow;
	[Export] Sprite2D leftArrow;
    [Export] Sprite2D iconSprite;
    [Export] CompressedTexture2D playerTexture;
	[Export] CompressedTexture2D confusedPlayerTexture;

    bool confused = false;
    public bool Confused{
        get{ return confused; }
		set{
            confused = value;
            iconSprite.Texture = confused ? confusedPlayerTexture : playerTexture;
        }
    }

	public void UpdateArrowDirection(){
		switch(level.playerDir){
			case Direction.Up:
                upArrow.Visible = true;
				rightArrow.Visible = false;
				downArrow.Visible = false;
				leftArrow.Visible = false;
                break;
			case Direction.Right:
				upArrow.Visible = false;
				rightArrow.Visible = true;
				downArrow.Visible = false;
				leftArrow.Visible = false;
                break;
			case Direction.Down:
				upArrow.Visible = false;
				rightArrow.Visible = false;
				downArrow.Visible = true;
				leftArrow.Visible = false;
                break;
			case Direction.Left:
				upArrow.Visible = false;
				rightArrow.Visible = false;
				downArrow.Visible = false;
				leftArrow.Visible = true;
                break;
        }
	}
}
