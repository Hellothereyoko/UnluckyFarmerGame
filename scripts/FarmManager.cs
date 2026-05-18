using Godot;
using System.Collections.Generic;


// So I was thinking it would be best to have a designated area where the player can grow crops, and
// as the levelss progress, the player can purchase and open up more land/soil to be able to grow even more crops
public partial class FarmManager : TileMapLayer
{
	[Export]
	public PackedScene CropScene;

	[Export]
	public CropData StartingCrop;
	
	[Export] 
	public CropData CarrotCrop;
	[Export] 
	public CropData PumpkinCrop;
	[Export]
	public CropData StrawberryCrop;
	[Export]
	public CropData CauliflowerCrop;

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
		// 4 = Carrot selected
		// 5 = pumpkin selected
		// 6 = strawberry selected
		// 7 = Cauliflower selected

		
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
		
		if (Input.IsKeyPressed(Key.Key4))
	{
		StartingCrop = CarrotCrop;
		GD.Print("Carrot selected");
	}

	if (Input.IsKeyPressed(Key.Key5))
	{
		StartingCrop = PumpkinCrop;
		GD.Print("Pumpkin selected");
	}

	if (Input.IsKeyPressed(Key.Key6))
	{
		StartingCrop = StrawberryCrop;
		GD.Print("Strawberry selected");
	}

	if (Input.IsKeyPressed(Key.Key7))
	{
		StartingCrop = CauliflowerCrop;
		GD.Print("Cauliflower selected");
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
