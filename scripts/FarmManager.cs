using Godot;
using System.Collections.Generic;

// So I was thinking it would be best to have a designated area where the player can grow crops, and
// as the levels progress, the player can purchase and open up more land/soil to be able to grow even more crops
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
	// tile position -> crop object
	private Dictionary<Vector2I, Crop> plantedCrops = new();

	public override void _Ready()
	{
		cropContainer = GetNode<Node2D>("../CropContainer");
		farmBounds = GetNode<TileMapLayer>("../FarmBounds");
	}

	public override void _Process(double delta)
	{
		// Tool switching
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

		// Crop selection
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

			Vector2I tilePos =
				LocalToMap(ToLocal(player.GlobalPosition));

			HandleTileInteraction(tilePos);
		}
	}

	private void HandleTileInteraction(Vector2I tilePos)
	{
		// Prevent farming outside farm area
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
		// Already tilled
		if (GetCellSourceId(tilePos) != -1)
			return;

		// Place tilled soil tile
		SetCell(tilePos, 0, Vector2I.Zero);

		GD.Print("Soil tilled!");
	}

	private void PlantCrop(Vector2I tilePos)
	{
		// Prevent duplicate crops
		if (plantedCrops.ContainsKey(tilePos))
			return;

		// Must be tilled first
		if (GetCellSourceId(tilePos) == -1)
			return;

		Crop crop = CropScene.Instantiate<Crop>();

		// IMPORTANT:
		// Set crop data BEFORE AddChild()
		crop.Data = StartingCrop;

		cropContainer.AddChild(crop);

		// Position crop on tile
		crop.GlobalPosition =
			ToGlobal(MapToLocal(tilePos));

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
