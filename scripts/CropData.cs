using Godot;

[GlobalClass]
public partial class CropData : Resource
{
	[Export] public string CropName = "";
	[Export] public float GrowTime = 10.0f;
	[Export] public Texture2D SeedTexture;
	[Export] public Texture2D GrownTexture;

	// Move scale and offset here instead of hardcoding in Crop.cs
	[Export] public Vector2 SpriteScale = Vector2.One;
	[Export] public Vector2 SpriteOffset = Vector2.Zero;
}
