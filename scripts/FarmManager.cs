using Godot;
using System.Collections.Generic;

public partial class FarmManager : TileMapLayer
{
	[Export]
	public PackedScene CropScene;

	[Export]
	public CropData StartingCrop;

	private Node2D cropContainer;	
	private TileMapLayer farmBounds;


	// Tracks planted crops by tile position
	// Example:
	// tile position -> crop object
	private Dictionary<Vector2I, Crop> plantedCrops = new();

	public override void _Ready()
	{
		cropContainer = GetNode<Node2D>("../CropContainer");
		farmBounds = GetNode<TileMapLayer>("../FarmBounds");
	}

	public override void _Process(double delta)
	{
		// Temporary tool switching using keyboard keys
		// 1 = Hoe
		// 2 = Seeds
		// 3 = Empty hands (harvest)

		
		if (Input.IsKeyPressed(Key.Key1))
		{
			ToolManager.CurrentTool = ToolType.Hoe;
			GD.Print("Hoe equipped");
		}

		if (Input.IsKeyPressed(Key.Key2))
		{
			ToolManager.CurrentTool = ToolType.Seeds;
			GD.Print("Seeds equipped");
		}

		if (Input.IsKeyPressed(Key.Key3))
		{
			ToolManager.CurrentTool = ToolType.None;
			GD.Print("Hands equipped");
		}
	
		
		if (Input.IsActionJustPressed("interact"))
		{
			Player player = GetNode<Player>("../Player");

			Vector2I tilePos = LocalToMap(ToLocal(player.GlobalPosition));

			HandleTileInteraction(tilePos);
		}
	}

	private void HandleTileInteraction(Vector2I tilePos)
{
	// Prevent farming outside the allowed farm area
	if (farmBounds.GetCellSourceId(tilePos) == -1)
	{
		GD.Print("Cannot farm here!");
		return;
	}

	if (ToolManager.CurrentTool == ToolType.Hoe)
	{
		TillSoil(tilePos);
	}
	else if (ToolManager.CurrentTool == ToolType.Seeds)
	{
		PlantCrop(tilePos);
	}
	else if (ToolManager.CurrentTool == ToolType.None)
	{
		HarvestCrop(tilePos);
	}
}

private void TillSoil(Vector2I tilePos)
{
	// Do not till already tilled soil
	if (GetCellSourceId(tilePos) != -1)
		return;

	// Place tilled dirt tile
	SetCell(tilePos, 0, Vector2I.Zero);

	GD.Print("Soil tilled!");
}
private void PlantCrop(Vector2I tilePos)
{
	// Prevent planting multiple crops on same tile
	if (plantedCrops.ContainsKey(tilePos))
		return;

	// Cannot plant on untilled soil
	if (GetCellSourceId(tilePos) == -1)
		return;
	
	
	Crop crop = CropScene.Instantiate<Crop>();

	cropContainer.AddChild(crop);

		// Position crop on tile
		// Offset manually adjusted for sprite alignment
	crop.GlobalPosition =
		ToGlobal(MapToLocal(tilePos)) + new Vector2(-8, -8);

	crop.Data = StartingCrop;

	plantedCrops.Add(tilePos, crop);

	GD.Print("Crop planted!");
}

private void HarvestCrop(Vector2I tilePos)
{
	if (!plantedCrops.ContainsKey(tilePos))
		return;

	plantedCrops[tilePos].Harvest();

	plantedCrops.Remove(tilePos);

	EraseCell(tilePos);

	GD.Print("Crop harvested!");
}

}
