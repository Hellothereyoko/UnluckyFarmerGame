using Godot;

[GlobalClass]
public partial class CropData : Resource
{
	[Export]
	public string CropName = "";

	[Export]
	public float GrowTime = 10.0f;

	[Export]
	public Texture2D SeedTexture;

	[Export]
	public Texture2D GrownTexture;
}
