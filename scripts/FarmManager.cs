using Godot;
using System.Collections.Generic;

public partial class FarmManager : TileMapLayer
{
	[Export]
	public PackedScene CropScene;

	[Export]
	public CropData StartingCrop;

	private Node2D cropContainer;

	private Dictionary<Vector2I, Crop> plantedCrops = new();

	public override void _Ready()
	{
		cropContainer = GetNode<Node2D>("../CropContainer");
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("interact"))
		{
			Vector2 mousePosition = GetGlobalMousePosition();

			Vector2I tilePos = LocalToMap(mousePosition);

			HandleTileInteraction(tilePos);
		}
	}

	private void HandleTileInteraction(Vector2I tilePos)
	{
		if (!plantedCrops.ContainsKey(tilePos))
		{
			TillAndPlant(tilePos);
		}
		else
		{
			plantedCrops[tilePos].Harvest();

			plantedCrops.Remove(tilePos);

			EraseCell(tilePos);
		}
	}

	private void TillAndPlant(Vector2I tilePos)
	{
		SetCell(tilePos, 0, Vector2I.Zero);

		Crop crop = CropScene.Instantiate<Crop>();

		cropContainer.AddChild(crop);

		crop.GlobalPosition = ToGlobal(MapToLocal(tilePos));

		crop.Data = StartingCrop;

		plantedCrops.Add(tilePos, crop);

		GD.Print("Crop planted at: " + tilePos);
	}
}
